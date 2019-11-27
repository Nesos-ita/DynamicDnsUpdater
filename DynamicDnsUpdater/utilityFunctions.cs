using System;
using System.Text;
using System.Windows.Forms;

using TaskScheduler;
using System.IO;
using System.Net;
using Microsoft.Win32;
using System.Security.Principal;
using System.Net.NetworkInformation;
using System.Collections.Generic;

namespace DynamicDnsUpdater
{
    class utilityFunctions
    {
        static StreamWriter streamLog = null;
        static StreamWriter streamLogLocal = null;
        struct timerWait
        {
            public DateTime myTime;
            public DateTime oldTime;
        };
        const string taskName = "DynamicDnsUpdater"; //both regedit and schtasks
        //const string updateLinkIPv6= "https://update6.dedyn.io"; //NOT USED NOW
        const string checkIpLink = "https://checkip.dedyn.io";
        List<string> oldNetCardIdAndIp = null; //used by IpIdChangedSinceLastCall
        int ipBasedUpdaterRateLimiter = 3;//change also max "lives" in the wait timer

        /// <summary>
        /// Check if user is administrator
        /// </summary>
        /// <returns></returns>
        public bool IsUserAdmin()
        {
            bool isAdmin;
            try
            {
                WindowsIdentity user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception)
            {
                isAdmin = false;
            }
            return isAdmin;
        }

        /// <summary>
        /// Returns program path (and exe name if needed), path ends with '\' but doesn't contain ""
        /// </summary>
        /// <returns></returns>
        public string GetExePath(bool includeProgramName = false)
        {
            string exePath = Application.ExecutablePath;//damn .net framework that doesnt sanitize the path!
            exePath = exePath.Replace('/', '\\');
            if (includeProgramName == false)
                exePath = exePath.Substring(0, exePath.LastIndexOf('\\') + 1);
            return exePath;
        }

        /// <summary>
        /// Returns program path (and exe name if needed), path ends with '\' and include "" when needed (if space is present)
        /// </summary>
        /// <param name="includeProgramName"></param>
        /// <returns></returns>
        public string GetSanitizedExePath(bool includeProgramName = false)
        {
            string exePath = GetExePath(includeProgramName);
            if (exePath.Contains(" ") == true)
            {
                exePath = exePath.Insert(0, "\"");
                exePath = exePath.Insert(exePath.Length, "\""); //sanitize path; otherwise we might read/write wrong file
            }
            return exePath;
        }

        /// <summary>
        /// Opens/create the log file
        /// </summary>
        void InitLog()
        {
            try
            {
                string tmppath = Path.GetTempPath();
                streamLog = new StreamWriter(tmppath + AppSettings.logFileName, true);
                streamLog.AutoFlush = true;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Opens/create the log file
        /// </summary>
        void InitLogLocal()
        {
            try
            {
                streamLogLocal = new StreamWriter(GetExePath() + AppSettings.logFileName, true);
                streamLogLocal.AutoFlush = true;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Adds a line to the log file (if opened)
        /// </summary>
        /// <param name="str">message to be logged</param>
        public void AddLog(string str)
        {
            if (AppSettings.logSetting == AppSettings.logSettingEnum.logHere)
            {
                if (streamLogLocal == null)
                    InitLogLocal();
                if (streamLogLocal != null)
                {
                    try
                    {
                        DateTime t = DateTime.Now;
                        streamLogLocal.WriteLine(t.ToString() + " - " + str);
                    }
                    catch (Exception) { }
                }
            }
            if (AppSettings.logSetting == AppSettings.logSettingEnum.logTemp)
            {
                if (streamLog == null)
                    InitLog();
                if (streamLog != null)
                {
                    try
                    {
                        DateTime t = DateTime.Now;
                        //string ora = t.Day.ToString().PadLeft(2, '0') + "-" + t.Month.ToString().PadLeft(2, '0') + "-" + t.Year.ToString().PadLeft(4, '0') + "," + t.Hour.ToString().PadLeft(2, '0') + "-" + t.Minute.ToString().PadLeft(2, '0') + "-" + t.Second.ToString().PadLeft(2, '0') + " - ";
                        streamLog.WriteLine(t.ToString() + " - " + str);
                    }
                    catch (Exception) { }
                }
            }
        }

        /// <summary>
        /// PC Clock based wait
        /// </summary>
        /// <param name="t">structure that hold current and old time</param>
        /// <param name="seconds">seconds to wait</param>
        /// <returns></returns>
        bool Wait(ref timerWait t, uint seconds)
        {
            if (t.oldTime.ToBinary() == 0)
                t.oldTime = DateTime.Now; //init
            t.myTime = DateTime.Now;
            TimeSpan diff = t.myTime.Subtract(t.oldTime);
            if ((uint)diff.TotalSeconds < seconds)
                return false; //not yet
            t.oldTime = t.myTime; //reset
            return true; //done
        }

        /// <summary>
        /// Return IP address (2 sec timeout)
        /// </summary>
        /// <returns></returns>
        public IPAddress GetIP()
        {
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(checkIpLink));
            webRequest.Timeout = 2000;
            webRequest.Method = "GET";
            webRequest.ContentType = "text/plain";
            webRequest.ContentLength = 0;
            try
            {
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                if (webResponse.StatusCode != HttpStatusCode.OK)
                    return null;
                StreamReader reader = new StreamReader(webResponse.GetResponseStream());
                string result = reader.ReadToEnd();
                reader.Close();
                if (result.Length > 15)
                    return null;
                IPAddress ip;
                if (IPAddress.TryParse(result, out ip) == false)
                    return null;
                return ip;
            }
            catch (Exception) { }
            return null;
        }

        public enum UpdateStatus { OK, NotConnected, Firewalled, UserNotFound, Unauthorized, UpdateFailed, InvalidUpdateLink, UnknownError };
        /// <summary>
        /// Updates the DDNS
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="hostname"></param>
        /// <param name="updateLink"></param>
        /// <returns></returns>
        public UpdateStatus UpdateDns(string user, string password, string hostname, string updateLink)
        {
            try
            {
                HttpWebRequest webRequest = null;
                try
                {
                    webRequest = (HttpWebRequest)WebRequest.Create(new Uri("https://" + updateLink + "?hostname=" + hostname));
                }
                catch (UriFormatException ex)
                {
                    AddLog("Update link error: " + ex.Message);
                    return UpdateStatus.InvalidUpdateLink;
                }
                webRequest.Timeout = 10000; //timeout for the operation, wait no more than 10 seconds for an answer
                webRequest.Method = "GET";
                webRequest.ContentType = "text/plain";
                webRequest.ContentLength = 0;
                webRequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + password))); //HTTP Basic Authentication
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    AddLog("Error, WebResponse returned: " + webResponse.StatusCode.ToString()); //probably never ends here since it throw exceptions
                    return UpdateStatus.UnknownError;
                }
                StreamReader reader = new StreamReader(webResponse.GetResponseStream());
                long slen = webResponse.ContentLength; //long bad answers could "fill the ram" (probably no need to fix)
                string result = reader.ReadToEnd();
                reader.Close();
                if (result == null || result == "")
                {
                    AddLog("Error, server null or empty response");
                    return UpdateStatus.UpdateFailed;
                }
                if (result.StartsWith("good", StringComparison.OrdinalIgnoreCase) == true || (result.StartsWith("nochg", StringComparison.OrdinalIgnoreCase) == true)) //compare + ignore case (start with? good || nochg)
                    return UpdateStatus.OK;
                else
                {
                    int len = result.Length;
                    if (len > 255)
                        len = 255; //avoid abusing error log, short messages can't fill the hdd
                    AddLog("Error, server says: " + result.Substring(0, len));
                    return UpdateStatus.UpdateFailed;
                }
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ConnectFailure:
                        AddLog("Error, ConnectFailure: " + ex.Message);
                        return UpdateStatus.Firewalled;
                    //break; //break is not needed since we immediatly return but we keep it here in case new lines will be added
                    case WebExceptionStatus.NameResolutionFailure:
                        AddLog("Error, NameResolutionFailure: " + ex.Message);
                        return UpdateStatus.NotConnected;
                    //break;
                    case WebExceptionStatus.ProtocolError:
                        HttpWebResponse r = (HttpWebResponse)ex.Response;
                        if (r == null)
                        {
                            AddLog("Protocol error, response is null");
                            return UpdateStatus.UnknownError;
                        }
                        switch (r.StatusCode)
                        {
                            case HttpStatusCode.Unauthorized:
                                return UpdateStatus.Unauthorized;
                            //break;
                            case HttpStatusCode.NotFound:
                                return UpdateStatus.UserNotFound;
                            //break;
                            default: //unhandled exception
                                AddLog("Error, WebResponse returned: " + r.StatusCode.ToString());
                                AddLog("UPDATE Exception: " + ex.Message);
                                if (ex.InnerException != null)
                                    AddLog("UPDATE Exception inner: " + ex.InnerException.Message);
                                return UpdateStatus.UnknownError;
                        }
                    //break; //end of protocol error case
                    case WebExceptionStatus.Timeout:
                        AddLog("Error, ConnectFailure, timeout: " + ex.Message);
                        return UpdateStatus.Firewalled;
                    //break;
                    default: //unhandled exception
                        AddLog("UPDATE Exception, unknown status: " + ex.Message);
                        if (ex.InnerException != null)
                            AddLog("UPDATE Exception inner, unknown status: " + ex.InnerException.Message);
                        return UpdateStatus.UnknownError;
                }
            }
            catch (Exception ex)
            {
                AddLog("UPDATE Exception: " + ex.Message);
                if (ex.InnerException != null)
                    AddLog("UPDATE Exception inner: " + ex.InnerException.Message);
            }
            return UpdateStatus.UnknownError;
        }

        /// <summary>
        /// Adds an autorun entry in Regedit or Tasks Scheduler
        /// </summary>
        /// <param name="userMode"></param>
        /// <param name="program"></param>
        /// <param name="arguments"></param>
        public bool AddTask(bool userMode, string program, string arguments = null)
        {
            if (userMode == true)
            {
                try
                {
                    RegistryKey k = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run\", true);
                    if (arguments == null)
                        k.SetValue(taskName, program, RegistryValueKind.String);
                    else
                        k.SetValue(taskName, program + " " + arguments, RegistryValueKind.String);
                    k.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    AddLog("Add user task error: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    //SCHTASKS.exe /Create /SC DAILY /TN report /TR notepad.exe
                    TaskScheduler.TaskScheduler scheduler = new TaskScheduler.TaskScheduler();
                    scheduler.Connect(null, null, null, null);

                    ITaskDefinition task = scheduler.NewTask(0);
                    task.RegistrationInfo.Author = "Dynamic Dns Updater";
                    task.RegistrationInfo.Description = "Keeps the Dynamic DNS updated";
                    task.Settings.AllowDemandStart = true;
                    task.Settings.Hidden = false;
                    task.Settings.RestartInterval = "PT1M"; //on error retry every minute 10 times
                    task.Settings.RestartCount = 10;
                    task.Settings.StartWhenAvailable = true;
                    task.Settings.DisallowStartIfOnBatteries = false; // start the task also if pc is on battery.
                    task.Settings.StopIfGoingOnBatteries = false;
                    task.Settings.ExecutionTimeLimit = "PT0S"; // infinite.
                    task.Settings.WakeToRun = false;
                    task.Principal.RunLevel = _TASK_RUNLEVEL.TASK_RUNLEVEL_HIGHEST;
                    task.Triggers.Create(_TASK_TRIGGER_TYPE2.TASK_TRIGGER_BOOT);

                    IExecAction action = (IExecAction)task.Actions.Create(_TASK_ACTION_TYPE.TASK_ACTION_EXEC); // the type of action to run .exe, there are others to send mail and show msg, both already depracated by Microsoft.
                    action.Path = program;
                    if (arguments != null)
                        action.Arguments = arguments;
                    //action.WorkingDirectory = "C:\\windows\\system32";

                    ITaskFolder root = scheduler.GetFolder("\\");
                    IRegisteredTask regTask = root.RegisterTaskDefinition(taskName, task, (int)_TASK_CREATION.TASK_CREATE_OR_UPDATE, "NT AUTHORITY\\SYSTEM", null, _TASK_LOGON_TYPE.TASK_LOGON_NONE, "");
                    //IRunningTask runTask = regTask.Run(null); // this will run just created task (run on demand).
                    return true;
                }
                catch (Exception ex)
                {
                    AddLog("Add admin task error: " + ex.Message);
                }
            }
            return false;
        }

        /// <summary>
        /// Deletes the task
        /// </summary>
        /// <param name="userMode"></param>
        /// <returns></returns>
        public bool DeleteTask(bool userMode)
        {
            if (userMode == true)
            {
                try
                {
                    RegistryKey k = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run\", true);
                    k.DeleteValue(taskName);
                    k.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    AddLog("Delete user task error: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    TaskScheduler.TaskScheduler scheduler = new TaskScheduler.TaskScheduler();
                    scheduler.Connect(null, null, null, null);
                    ITaskFolder root = scheduler.GetFolder("\\");
                    root.DeleteTask(taskName, 0);
                    return true;
                }
                catch (Exception ex)
                {
                    AddLog("Delete admin task error: " + ex.Message);
                }
            }
            return false;
        }

        /// <summary>
        /// Check if a task exists
        /// </summary>
        /// <param name="userMode"></param>
        /// <returns></returns>
        public bool TaskExist(bool userMode)
        {
            try
            {
                if (userMode == true)
                {
                    RegistryKey k = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run\", false);
                    string val = (string)k.GetValue(taskName, null, RegistryValueOptions.None);
                    k.Close();
                    if (val != null)
                        return true;
                }
                else
                {
                    TaskScheduler.TaskScheduler scheduler = new TaskScheduler.TaskScheduler();
                    scheduler.Connect(null, null, null, null);
                    ITaskFolder root = scheduler.GetFolder("\\");
                    IRegisteredTask task = root.GetTask(taskName);
                    if (task != null)
                        return true;
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        /// <summary>
        /// returns interfaces that are up and has type: ethernet or wifi
        /// </summary>
        /// <returns></returns>
        public List<NetworkInterface> ListActiveInterfaces()
        {
            List<NetworkInterface> validInterfaces = new List<NetworkInterface>();
            try
            {
                NetworkInterface[] interfcaces = NetworkInterface.GetAllNetworkInterfaces();
                for (int i = 0; i < interfcaces.Length; i++)
                {
                    //less restrictive filter might be != NetworkInterfaceType.Loopback, now is strict compared to GetIsNetworkAvailable()
                    if (interfcaces[i].OperationalStatus == OperationalStatus.Up && (interfcaces[i].NetworkInterfaceType == NetworkInterfaceType.Ethernet || interfcaces[i].NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
                        validInterfaces.Add(interfcaces[i]);
                }
            }
            catch (Exception)
            {
                return new List<NetworkInterface>();
            }
            return validInterfaces;
        }

        /// <summary>
        /// returns ipv4 of the interface (the first internet ipv4)
        /// </summary>
        /// <param name="netInterface"></param>
        /// <returns></returns>
        public IPAddress QueryInterfaceIP(NetworkInterface netInterface)
        {
            IPAddress ip = null;
            try
            {
                IPInterfaceProperties ipInfo = netInterface.GetIPProperties();
                for (int i = 0; i < ipInfo.UnicastAddresses.Count; i++)
                {
                    if (ipInfo.UnicastAddresses[i].Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ip = ipInfo.UnicastAddresses[i].Address;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
            return ip;
        }

        /// <summary>
        /// returns a unique identifier string composed by cardid+ip
        /// </summary>
        /// <returns></returns>
        public List<string> GetInterfaceIdIpList()
        {
            List<NetworkInterface> li = ListActiveInterfaces();
            if (li.Count == 0)
                return new List<string>();
            List<string> idip = new List<string>();
            for (int i = 0; i < li.Count; i++)
            {
                IPAddress ip = QueryInterfaceIP(li[i]);
                string ipStr = "";
                if (ip != null)
                    ipStr = ip.ToString();
                idip.Add(li[i].Id.ToString() + ipStr);
            }
            return idip;
        }

        /// <summary>
        /// returns true if the network status (id+ip) is not equal to the old one and there is a connection, false if status is not changed, there are 0 connections, first call
        /// </summary>
        /// <returns></returns>
        public bool IpIdChangedSinceLastCall()
        {
            bool ipOrNetworkCardChanged = false;
            List<string> currentNetCardIdAndIp = GetInterfaceIdIpList();
            if (oldNetCardIdAndIp == null)
            {
                oldNetCardIdAndIp = currentNetCardIdAndIp;
                return false; //first start
            }
            if (oldNetCardIdAndIp.Count == currentNetCardIdAndIp.Count)
            {
                for (int i = 0; i < oldNetCardIdAndIp.Count; i++)
                {
                    if (oldNetCardIdAndIp.Contains(currentNetCardIdAndIp[i]) == false)//check if ip or network card is changed
                        ipOrNetworkCardChanged = true;
                }
            }
            else
                ipOrNetworkCardChanged = true; //number of active network cards changed, update ddns
            if (currentNetCardIdAndIp.Count == 0)
                ipOrNetworkCardChanged = false;//connected to disconnected is a change, but a useless one
            oldNetCardIdAndIp = currentNetCardIdAndIp;
            return ipOrNetworkCardChanged;
        }

        /// <summary>
        /// Wait the desidered time and update DNS
        /// </summary>
        public void WaitAndUpdate()
        {
            UpdateStatus ret;
            timerWait tmrTimedUpdate = new timerWait();
            timerWait tmrIpChangeDetect = new timerWait();
            timerWait tmrIpChangeUpdate = new timerWait();
            bool ipChanged = false;
            timerWait tmrRateLimiter = new timerWait();

            ret = UpdateDns(AppSettings.user, AppSettings.password, AppSettings.hostname, AppSettings.updateLink);
            AppSettings.lastUpdateStatus = ret;
            AppSettings.lastUpdateStatusChanged = true;
            AddLog("Updating DNS: " + ret.ToString());

            while (AppSettings.exitUpdateLoop == false)
            {
                if (Wait(ref tmrTimedUpdate, AppSettings.updateInterval * 60) == true)//default updateInterval is 60 min
                {
                    ret = UpdateDns(AppSettings.user, AppSettings.password, AppSettings.hostname, AppSettings.updateLink);
                    AppSettings.lastUpdateStatus = ret;
                    AppSettings.lastUpdateStatusChanged = true;
                    AddLog("Updating DNS: " + ret.ToString());
                    oldNetCardIdAndIp = null;//prevent next ip change check from trigger: if you boot from standby after long time and every timer is expired, probably you are also reconnecting, no need to update twice
                }
                if (AppSettings.checkAlsoLocalIpChange == true)
                {
                    if (Wait(ref tmrIpChangeDetect, 30) == true)//every 30 second check for local ip change
                    {
                        ipChanged = IpIdChangedSinceLastCall();
                        tmrIpChangeUpdate.oldTime = DateTime.Now;//reset timer
                    }
                    if (Wait(ref tmrIpChangeUpdate, 15) == true && ipChanged == true && ipBasedUpdaterRateLimiter > 0)//after 15 second do the actual update
                    {
                        //if status changed and i have lives, update ddns. 30sec delay to ensure you are connected after network change
                        ipChanged = false;//disable timer
                        ipBasedUpdaterRateLimiter--;//use a life
                        ret = UpdateDns(AppSettings.user, AppSettings.password, AppSettings.hostname, AppSettings.updateLink);
                        AppSettings.lastUpdateStatus = ret;
                        AppSettings.lastUpdateStatusChanged = true;
                        tmrTimedUpdate.oldTime = DateTime.Now;//reset timer, we updated just now because of ip change, so reset timed update
                        if (ipBasedUpdaterRateLimiter>0)
                            AddLog("Ip changed, updating DNS: " + ret.ToString());
                        else
                            AddLog("Ip changed (rate limited), updating DNS: " + ret.ToString());
                    }
                    if (Wait(ref tmrRateLimiter, 10 * 60) == true)//every 10 minutes add a "life" to rate limiter counter
                    {
                        if (ipBasedUpdaterRateLimiter < 3)
                            ipBasedUpdaterRateLimiter++;//this means that you have 3 fast(30 sec) updates and then you will be rate limited to 1 every 5 minutes, in case of slower update you eventually regain all 3 fast updates
                    }
                }
                System.Threading.Thread.Sleep(1000);//1 sec
            }
        }

        /// <summary>
        /// Reads settings, returns false on any error
        /// </summary>
        /// <returns></returns>
        public bool ReadSettings()
        {
            string path = GetExePath();
            path += AppSettings.settingsFileName;
            if (File.Exists(path) == true)
            {
                try
                {
                    FileStream f = new FileStream(path, FileMode.Open, FileAccess.Read);
                    StreamReader reader = new StreamReader(f, Encoding.UTF8);
                    string user = reader.ReadLine();
                    string password = reader.ReadLine();
                    string hostname = reader.ReadLine();
                    string updateLink = reader.ReadLine();
                    uint updateInterval = Convert.ToUInt32(reader.ReadLine());
                    if (updateInterval < 1)
                        updateInterval = 1;
                    if (updateInterval > 1440)
                        updateInterval = 1440; //fix min & max

                    string logOption = "nolog";
                    if (reader.EndOfStream == false)//compatibility with older version (where there wasn't log setting)
                        logOption = reader.ReadLine().ToLower();
                    bool checkIpUpdate = false;
                    if (reader.EndOfStream == false)//compatibility with older version (where there wasn't ip setting)
                        checkIpUpdate = Convert.ToBoolean(reader.ReadLine());
                    if (reader.EndOfStream == false)
                        throw new Exception("Incorrect file format, too long");
                    reader.Close(); //if we reach this point settings has been sucesfully read

                    AppSettings.logSettingEnum tempLogStatus = AppSettings.logSettingEnum.noLog;
                    switch (logOption)
                    {
                        case "nolog":
                            //nolog is default
                            break;
                        case "loghere":
                            tempLogStatus = AppSettings.logSettingEnum.logHere;
                            break;
                        case "logtemp":
                        case "log":
                            tempLogStatus = AppSettings.logSettingEnum.logTemp;
                            break;
                        default:
                            throw new Exception("Incorrect file format, unknown log option");
                    }
                    //---change setting only now that we know they are valid---
                    AppSettings.user = user; //apply settings
                    AppSettings.password = password;
                    AppSettings.hostname = hostname;
                    AppSettings.updateLink = updateLink;
                    AppSettings.updateInterval = updateInterval;
                    AppSettings.originalLogSetting = tempLogStatus;
                    if (AppSettings.overrideLogOption == false)//log option from cmdline, don't use file settings
                        AppSettings.logSetting = tempLogStatus;
                    AppSettings.checkAlsoLocalIpChange = checkIpUpdate;
                    AppSettings.firstRun = false;
                    AddLog("Settings read OK");
                    return true;
                }
                catch (Exception ex)
                {
                    AddLog("Error reading settings: " + ex.Message);
                }
            }
            return false;
        }

        /// <summary>
        /// Save settings
        /// </summary>
        public bool SaveSettings()
        {
            string path = GetExePath();
            path += AppSettings.settingsFileName;
            try
            {
                FileStream f = new FileStream(path, FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(f, Encoding.UTF8);
                writer.WriteLine(AppSettings.user);
                writer.WriteLine(AppSettings.password);
                writer.WriteLine(AppSettings.hostname);
                writer.WriteLine(AppSettings.updateLink);
                writer.WriteLine(AppSettings.updateInterval.ToString());
                if (AppSettings.overrideLogOption == false)
                    writer.WriteLine(AppSettings.logSetting.ToString());//devo scrivere l'originale
                else
                    writer.WriteLine(AppSettings.originalLogSetting.ToString());
                writer.WriteLine(AppSettings.checkAlsoLocalIpChange);
                writer.Close();
                AddLog("Settings saved OK");
                return true;
            }
            catch (Exception ex)
            {
                AddLog("Error saving settings: " + ex.Message);
            }
            return false;
        }
    }
}
