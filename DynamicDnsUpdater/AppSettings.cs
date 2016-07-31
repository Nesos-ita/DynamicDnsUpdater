namespace DynamicDnsUpdater
{
    static class AppSettings
    {
        //Settings from file
        public static string user = "";
        public static string password = "";
        public static string hostname = "";
        public static string updateLink = "";
        public static uint updateInterval=1;
        public static bool firstRun = true;

        //Settings from cmdLine
        public static bool logEnabled = false;

        //settings from regedit / SchTasks
        public static bool autorunUserEnabled = false;
        public static bool autorunAdminEnabled = false;

        //Settings internal
        public static bool exitUpdateLoop = false;
        public static utilityFunctions.UpdateStatus lastUpdateStatus=utilityFunctions.UpdateStatus.NotConnected;
        public static bool lastUpdateStatusChanged = false;
    }
}
