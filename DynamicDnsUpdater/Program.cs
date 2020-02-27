using System;
using System.Reflection;
using System.Windows.Forms;

namespace DynamicDnsUpdater
{
    static class Program
    {
        static utilityFunctions utils = new utilityFunctions();
        static bool backGroundMode = false;
        /// <summary>
        /// Check command line arguments and calls appropriate functions
        /// </summary>
        /// <param name="args">cmd line agruments</param>
        static bool ParseCmdLine(string[] args)
        {
            bool help = false, continueRunProgram = true;
            foreach (string arg in args)
            {
                string arglow = arg.ToLower();//case insensitive
                arglow = arglow.Replace('/', '-');//both allowed

                switch (arglow)
                {
                    case "-nolog":
                        if (AppSettings.overrideLogOption == false)
                        {
                            AppSettings.logSetting = AppSettings.logSettingEnum.noLog;
                            AppSettings.overrideLogOption = true;
                        }
                        break;
                    case "-log":
                    case "-logtemp":
                        if (AppSettings.overrideLogOption == false)
                        {
                            AppSettings.logSetting = AppSettings.logSettingEnum.logTemp;
                            AppSettings.overrideLogOption = true;
                        }
                        break;
                    case "-loghere":
                        if (AppSettings.overrideLogOption == false)
                        {
                            AppSettings.logSetting = AppSettings.logSettingEnum.logHere;
                            AppSettings.overrideLogOption = true;
                        }
                        break;
                    case "-bg":
                        backGroundMode = true;//runs in background mode (do not start the form)
                        break;
                    case "-help":
                        help = true;
                        continueRunProgram = false;
                        break;
                    default:
                        MessageBox.Show("Invalid command, try \"-help\". Error at argument: \"" + arglow + "\"");
                        continueRunProgram = false;
                        break;
                }
            }
            if (help == true) //showed only once
                MessageBox.Show(
                    "-help\tShow this message\n" +
                    "Note: if you specify a log option the corresponding file setting is ignored;\nlog setting in file is not changed\n" +
                    "-NoLog\tDisable log\n" +
                    "-LogTemp\tEnable logging to file %TEMP%\\" + AppSettings.logFileName + "\n" +
                    "-LogHere\tEnable logging to file in application directory\n" +
                    "-bg\tStarts in background mode (no GUI)"

                    , "Available commands (case insensitive):");
            return continueRunProgram;
        }

        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Embedded dll, avoid dependency
            AppDomain.CurrentDomain.AssemblyResolve += (sender, arg) => { if (arg.Name.StartsWith("Interop.TaskScheduler")) return Assembly.Load(Properties.Resources.Interop_TaskScheduler); return null; }; //embedded dependency
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Enforce using Tls1.2 or 1.3 before any connection (Tls 1.0 and 1.1 are disabled)
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls13 | System.Net.SecurityProtocolType.Tls12;

            //my pre-gui added code (command line)
            if (args.Length != 0)
                if (ParseCmdLine(args) == false)
                    return;
            if (utils.ReadSettings() == true)
                AppSettings.firstRun = false;
            utils.AddLog("Version: " + Application.ProductVersion);//here because if we log or not is readfrom file (or commandline)
            if (backGroundMode == true)
            {
                utils.AddLog("Background mode");
                if (AppSettings.firstRun == true)
                    utils.AddLog("Can't start in background mode without settings file"); //will go to "program closed"                
                else
                    utils.WaitAndUpdate(); //program will never exit/return from here
            }
            else
            {
                utils.AddLog("GUI mode");
                Application.Run(new MainForm());
            }
            utils.AddLog("Process closed");
        }
    }
}
