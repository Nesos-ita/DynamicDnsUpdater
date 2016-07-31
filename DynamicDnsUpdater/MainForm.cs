using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;

namespace DynamicDnsUpdater
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        utilityFunctions utils = new utilityFunctions();
        //Task updaterTask = null;
        Thread updaterThread = null;

        void StartUpdateTask()
        {
            AppSettings.exitUpdateLoop = false;
            //Action a = new Action(utils.WaitAndUpdate);
            //updaterTask = new Task(a);
            //updaterTask.Start();
            updaterThread = new Thread(new ThreadStart(utils.WaitAndUpdate));
            updaterThread.IsBackground = true; // or the app will never exit
            updaterThread.Start();
            statusImg.BackColor = ColorTranslator.FromHtml("#5AFF5A"); //green
            statusImg.Checked = true;
            lblStatus.Text = "Running";            
            utils.AddLog("Running");
        }

        void StopUpdateTask()
        {
            AppSettings.lastUpdateStatusChanged = false;
            AppSettings.exitUpdateLoop = true;
            lblStatus.Text = "Waiting the updater to stop...";
            this.Refresh(); //refresh status label
            //updaterTask.Wait(); // wait that the task finish, usually <1 sec, max 11 (long connection timeout)
            //updaterTask = null;            
            while (updaterThread!=null && updaterThread.IsAlive == true)
                Thread.Sleep(100);
            updaterThread = null;
            statusImg.BackColor = ColorTranslator.FromHtml("#FF5A5A"); //red
            statusImg.Checked = false;
            lblStatus.Text = "Stopped";
            utils.AddLog("Stopped");
        }

        void StatusUpdaterTask()
        {
            while (true)
            {
                if (AppSettings.lastUpdateStatusChanged == true)
                {
                    AppSettings.lastUpdateStatusChanged = false;
                    //on general error stop updater
                    //if connection error, keep trying
                    if (AppSettings.lastUpdateStatus != utilityFunctions.UpdateStatus.OK && AppSettings.lastUpdateStatus != utilityFunctions.UpdateStatus.NotConnected && AppSettings.lastUpdateStatus != utilityFunctions.UpdateStatus.Firewalled)
                        Invoke((MethodInvoker)delegate () { StopUpdateTask(); });
                    lblStatus.Text = "Update result: " + AppSettings.lastUpdateStatus.ToString();                    
                }
                System.Threading.Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// Starts the task which keeps the label updated
        /// </summary>
        void StartStatusUpdaterTask()
        {
            //Action a = new Action(StatusUpdaterTask);
            //Task t = new Task(a);
            //t.Start();
            Thread t = new Thread(new ThreadStart(StatusUpdaterTask));
            t.IsBackground = true;
            t.Start();
        }

        /// <summary>
        /// Set the apparence of button and textbox
        /// </summary>
        /// <param name="editMode"></param>
        void EditOKMode(bool editMode)
        {
            if (editMode==true)
            {
                btnEditOK.Text = "&OK";
                txtUser.Enabled = true;
                txtPassword.Enabled = true;
                txtHostname.Enabled = true;
                txtUpdateLink.Enabled = true;
                numInterval.Enabled = true;
                btnUpdateNow.Enabled = false;
                
            }
            else
            {
                btnEditOK.Text = "&Edit";
                txtUser.Enabled = false;
                txtPassword.Enabled = false;
                txtHostname.Enabled = false;
                txtUpdateLink.Enabled = false;
                numInterval.Enabled = false;
                btnUpdateNow.Enabled = true;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                trayIcon.Visible = true;
                trayIcon.ShowBalloonTip(1000);
                this.Hide();
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                trayIcon.Visible = false;
            }
        }

        private void tryIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            trayIcon.Visible = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            optAutorunUser.Checked = utils.TaskExist(true);
            optAutorunAdmin.Checked = utils.TaskExist(false);
            chkLog.Checked = AppSettings.logEnabled;
            if (utils.IsUserAdmin() == false)
                optAutorunAdmin.Enabled = false;
            if (optAutorunUser.Checked || optAutorunAdmin.Checked)
                optAutorunNo.Checked = false;

            if (AppSettings.firstRun==true)
            {
                EditOKMode(true); //set in edit mode and don't start updater task
                lblStatus.Text = "That (red) square shows if the Dynamic DNS updater task is running";
                utils.AddLog("First run mode (no settings file or can't open)");                
            }
            else
            {
                txtUser.Text = AppSettings.user;
                txtPassword.Text = AppSettings.password;
                txtHostname.Text = AppSettings.hostname;
                txtUpdateLink.Text = AppSettings.updateLink;
                numInterval.Value = AppSettings.updateInterval;
                StartUpdateTask(); //everything is correct so start the update task
            }
            StartStatusUpdaterTask(); //label updater task
            optAutorunNo.CheckedChanged += new System.EventHandler(optAutorunNo_CheckedChanged); //add only now the event handler; in this way when loading the UI and set the radio box we dont fire an event
            optAutorunAdmin.CheckedChanged += new System.EventHandler(optAutorunAdmin_CheckedChanged);
            optAutorunUser.CheckedChanged += new System.EventHandler(optAutorunUser_CheckedChanged);
        }

        private void btnEditOK_Click(object sender, EventArgs e)
        {
            bool editMode = txtUser.Enabled;
            if (editMode == true)
            {
                if (txtUser.Text == "" || txtPassword.Text == "" || txtHostname.Text == "" || txtUpdateLink.Text == "")
                {
                    MessageBox.Show("Please fill all the data before continuing", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                AppSettings.user = txtUser.Text;
                AppSettings.password = txtPassword.Text;
                AppSettings.hostname = txtHostname.Text;
                AppSettings.updateLink = txtUpdateLink.Text;
                AppSettings.updateInterval = Convert.ToUInt32(numInterval.Value);
                if (utils.SaveSettings() == false) //save new settings to file
                    MessageBox.Show("Error saving settings", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                StartUpdateTask(); //start task
                EditOKMode(false); //change apparence
            }
            else
            {
                StopUpdateTask(); //stop the updater task while editing
                EditOKMode(true);
            }
        }

        private void btnGetIP_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Obtaining current ip...";
            txtIP.Refresh(); //redraw text
            IPAddress ip = utils.GetIP();
            if (ip != null)
            {
                lblStatus.Text = "Current IP found";
                txtIP.Text = ip.ToString();
            }
            else
            {
                lblStatus.Text = "Error while getting current ip";
                txtIP.Text = "";
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (AppSettings.firstRun == true)
                MessageBox.Show("This is the first time you run the program\nI know it because there is no settings file (or it's invalid).\n\nJust fill the data and confirm;\nsettings will be permanently saved in a file;\ndon't worry, you can change them later if needed.\n\nProgram will start work as soon as you confirm settings.\n\nTIP: Hold the mouse over components to get a description\nTIP: Command line options available; try \"-help\"", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUpdateNow_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Updating...";
            this.Refresh();
            utilityFunctions.UpdateStatus ret = utils.UpdateDns(AppSettings.user, AppSettings.password, AppSettings.hostname, AppSettings.updateLink);
            lblStatus.Text = "Manual update result: " + ret.ToString();
            utils.AddLog("Manually updating DNS: " + ret.ToString());
        }

        private void btnResolveHostname_Click(object sender, EventArgs e)
        {
            if (txtHostResolve.Text=="")
            {
                lblStatus.Text = "Write something in the textbox before resolving";
                return;
            }
            lblStatus.Text = "Resolving...";
            this.Refresh();
            IPHostEntry h=null;
            try
            {
                h = Dns.GetHostEntry(txtHostResolve.Text);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error while resolving hostname: " + ex.Message;
                txtIpResolved.Text = "";
                return;
            }
            if (h==null)
            {
                lblStatus.Text = "Error while resolving hostname: HostEntry is null";
                txtIpResolved.Text = "";
                return;
            }
            IPAddress[] ipList = h.AddressList;
            if (ipList==null || ipList.Length==0)
            {
                lblStatus.Text = "Error while resolving hostname: no ip found";
                txtIpResolved.Text = "";
                return;
            }
            txtIpResolved.Text = ipList[0].ToString();
            lblStatus.Text = "Hostname resolved";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tryIcon_Click(object sender, EventArgs e)
        {
            MouseEventArgs m = (MouseEventArgs)e;
            if (m.Button==MouseButtons.Right)
                tryIconMenu.Show(Cursor.Position.X,Cursor.Position.Y);
        }

        private void txtIP_Click(object sender, EventArgs e)
        {
            txtIP.SelectAll();
        }

        private void txtIpResolved_Click(object sender, EventArgs e)
        {
            txtIpResolved.SelectAll();
        }

        private void chkLog_Click(object sender, EventArgs e)
        {
            AppSettings.logEnabled = chkLog.Checked;
            lblStatus.Text = "Logging enable status: " + chkLog.Checked;
        }
      
        private void optAutorunNo_CheckedChanged(object sender, EventArgs e)
        {
            if (optAutorunNo.Checked==true)
            {
                bool result1 = utils.DeleteTask(true);
                bool result2 = utils.DeleteTask(false);
                lblStatus.Text = "Remove tasks results: " + result1.ToString() + " " + result2.ToString();
            }
        }

        private void optAutorunAdmin_CheckedChanged(object sender, EventArgs e)
        {
            if (optAutorunAdmin.Checked == true)
            {
                string programNewPath = @"C:\Program Files\DynamicDnsUpdater\";
                string programNewExe = programNewPath + "DynamicDnsUpdater.exe";
                string spath = "\"" + programNewExe + "\"";
                if (MessageBox.Show("Program (and current settings) will be copied in " + spath + "\n\nKeep in mind that on boot the program will load the copied settings; so add the autorun entry only when you have correct (working) settings\nAre you sure you want to continue?\n\nWhy this?\nSo there is no privilege escalation bug and user can not replace the exe with something other to gain admin privileges", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    optAutorunAdmin.Checked = false;
                    optAutorunNo.Checked = true;
                    return;
                }
                try
                {
                    Directory.CreateDirectory(programNewPath);
                    string myExe = utils.GetExePath(true);
                    string currPath = utils.GetExePath(false);
                    try
                    {
                        File.Copy(myExe, programNewExe, true); //always try to overwrite (in case of an update) but ignore error so it will work also if file exists and is opened (can't replace)
                    }
                    catch (Exception){}
                    if (File.Exists(programNewExe) == false) //if there is no file is a problem
                        throw new Exception("Can't copy exe to destination");

                    if (File.Exists(currPath + "dduSettings.txt") == true)
                        File.Copy(currPath + "dduSettings.txt", programNewPath + "dduSettings.txt", true);
                    else
                        throw new Exception("Save settings before turning on autorun!");

                    bool result = utils.AddTask(false, spath, "-bg -log");
                    if (result == true)
                    {
                        utils.DeleteTask(true); //if add admin task (try to) remove the user one
                        MessageBox.Show("Will autostart on next boot", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    lblStatus.Text = "Add admin task result: " + result.ToString();
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Add admin task result: false";
                    optAutorunAdmin.Checked = false;
                    optAutorunNo.Checked = true;
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void optAutorunUser_CheckedChanged(object sender, EventArgs e)
        {
            if (optAutorunUser.Checked == true)
            {
                if (File.Exists(utils.GetExePath(false) + "dduSettings.txt")==false)
                {
                    optAutorunUser.Checked = false;
                    optAutorunNo.Checked = true;
                    MessageBox.Show("Save settings before turning on autorun!", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string path = utils.GetSanitizedExePath(true);
                MessageBox.Show("Keep in mind that if you move, rename or delete the program the autorun function will stop work.\nProgram now is here:\n" + path + "\n\nWhy this?\nSo you can place the program where you want and you don't need admin privileges.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                bool result = utils.AddTask(true, path, "-bg -log");
                if (result == true)
                {
                    utils.DeleteTask(false); //if add user task (try to) remove the admin one
                    MessageBox.Show("Will autostart on next login", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                lblStatus.Text = "Add user task result: " + result.ToString();
            }
        }

        private void btnOpenLog_Click(object sender, EventArgs e)
        {
            try
            {
                string logFile = Path.GetTempPath() + "ddu.log";
                if (File.Exists(logFile) == false)
                {
                    lblStatus.Text = "Log file doesn't exists";
                    return;
                }
                Process p = new Process();
                p.StartInfo.FileName = @"C:\windows\system32\notepad.exe";
                p.StartInfo.Arguments = logFile;
                p.Start();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Open log file error";
                MessageBox.Show("Error: " + ex.Message, "Problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            lblStatus.Text = "Log file opened";
        }
        
    }
}