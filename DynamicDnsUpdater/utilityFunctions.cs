using System;
using System.Text;
using System.Windows.Forms;

using TaskScheduler;
using System.IO;
using System.Net;
using Microsoft.Win32;
using System.Security.Principal;

namespace DynamicDnsUpdater
{
    class utilityFunctions
    {
        StreamWriter streamLog = null;
        struct timerWait
        {
            public DateTime myTime;
            public DateTime oldTime;
        };
        const string settingsFileName = "dduSettings.txt"; //settings file name
        const string taskName = "DynamicDnsUpdater"; //both regedit and schtasks
        const string logFileName = "ddu.log"; //placed in %TEMP%
        //const string updateLinkIPv6= "https://update6.dedyn.io"; //NOT USED NOW
        const string checkIpLink = "https://checkip.dedyn.io";

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
        public string GetExePath(bool includeProgramName=false)
        {
            string exePath = Application.ExecutablePath;//damn .net framework that doesnt sanitize the path!
            exePath = exePath.Replace('/', '\\');
            if (includeProgramName==false)
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
                streamLog = new StreamWriter(tmppath + logFileName, true);
                streamLog.AutoFlush = true;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Adds a line to the log file (if opened)
        /// </summary>
        /// <param name="str">message to be logged</param>
        public void AddLog(string str)
        {
            if (AppSettings.logEnabled == true)
            {
                if (streamLog == null)
                    InitLog();
                try
                {
                    DateTime t = DateTime.Now;
                    //string ora = t.Day.ToString().PadLeft(2, '0') + "-" + t.Month.ToString().PadLeft(2, '0') + "-" + t.Year.ToString().PadLeft(4, '0') + "," + t.Hour.ToString().PadLeft(2, '0') + "-" + t.Minute.ToString().PadLeft(2, '0') + "-" + t.Second.ToString().PadLeft(2, '0') + " - ";
                    streamLog.WriteLine(t.ToString() + " - " + str);
                }
                catch (Exception) { }
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
            t.oldTime = new DateTime(0); //reset
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
                if (result.StartsWith("good",StringComparison.OrdinalIgnoreCase)==true || (result.StartsWith("nochg", StringComparison.OrdinalIgnoreCase) == true)) //compare + ignore case (start with? good || nochg)
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
        public bool AddTask(bool userMode, string program,string arguments=null)
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
                    if (arguments!=null)
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
        /// Wait the desidered time and update DNS
        /// </summary>
        public void WaitAndUpdate()
        {
            UpdateStatus ret;
            timerWait t = new timerWait();

            ret = UpdateDns(AppSettings.user, AppSettings.password, AppSettings.hostname, AppSettings.updateLink);
            AppSettings.lastUpdateStatus = ret;
            AppSettings.lastUpdateStatusChanged = true;
            AddLog("Updating DNS: " + ret.ToString());

            while (AppSettings.exitUpdateLoop==false)
            {
                if (Wait(ref t, AppSettings.updateInterval*60) == true)//default updateInterval is 60 min
                {
                    ret = UpdateDns(AppSettings.user, AppSettings.password, AppSettings.hostname, AppSettings.updateLink);
                    AppSettings.lastUpdateStatus = ret;
                    AppSettings.lastUpdateStatusChanged = true;
                    AddLog("Updating DNS: " + ret.ToString());
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
            path += settingsFileName;
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
                    if (reader.EndOfStream == true)
                        throw new Exception("Incorrect file format, too short");
                    uint updateInterval = Convert.ToUInt32(reader.ReadLine());
                    if (updateInterval < 1)
                        updateInterval = 1;
                    if (updateInterval > 1440)
                        updateInterval = 1440; //fix min & max
                    if (reader.EndOfStream == false)
                        throw new Exception("Incorrect file format, too long");
                    reader.Close(); //if we reach this point settings has been sucesfully read
                    AppSettings.user = user; //apply settings
                    AppSettings.password = password;
                    AppSettings.hostname = hostname;
                    AppSettings.updateLink = updateLink;
                    AppSettings.updateInterval = updateInterval;
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
            path += settingsFileName;
            try
            {
                FileStream f = new FileStream(path, FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(f, Encoding.UTF8);
                writer.WriteLine(AppSettings.user);
                writer.WriteLine(AppSettings.password);
                writer.WriteLine(AppSettings.hostname);
                writer.WriteLine(AppSettings.updateLink);
                writer.WriteLine(AppSettings.updateInterval.ToString());
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
