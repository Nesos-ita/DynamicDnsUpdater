using System.Windows.Forms;

namespace DynamicDnsUpdater
{
    class MainProg
    {
        utilityFunctions utils = new utilityFunctions();
        bool backGroundMode = false;
        /// <summary>
        /// Check command line arguments and calls appropriate functions
        /// </summary>
        /// <param name="args">cmd line agruments</param>
        bool ParseCmdLine(string[] args)
        {
            bool help = false, continueRunProgram = true;
            foreach (string arg in args)
            {
                string arglow = arg.ToLower();//case insensitive
                arglow = arglow.Replace('/', '-');//both allowed

                switch (arglow)
                {
                    case "-log":
                        AppSettings.logEnabled = true;
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
                MessageBox.Show("-help\tShow this message\n-log\tEnable Logging to file %TEMP%\\ddu.log\n-bg\tStarts in background mode (no GUI)", "Available commands:");
            return continueRunProgram;    
        }

        /// <summary>
        /// EntryPoint of the program
        /// </summary>
        /// <param name="args"></param>
        public void Main(string[] args)
        {

            if (args.Length != 0)
                if (ParseCmdLine(args) == false)
                    return;
            utils.AddLog("Process started, version: " + Application.ProductVersion);
            if (utils.ReadSettings() == true)
                AppSettings.firstRun = false;

            if (backGroundMode == true)
            {
                utils.AddLog("Background mode");
                if (AppSettings.firstRun==true)
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
