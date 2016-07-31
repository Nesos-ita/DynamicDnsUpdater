using System;
using System.Reflection;
using System.Windows.Forms;

namespace DynamicDnsUpdater
{
    static class Program
    {
        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, arg) => { if (arg.Name.StartsWith("Interop.TaskScheduler")) return Assembly.Load(Properties.Resources.Interop_TaskScheduler); return null; }; //embedded dependency
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainProg p = new MainProg();
            p.Main(args);
        }
    }
}
