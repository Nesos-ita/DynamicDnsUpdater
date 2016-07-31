namespace DynamicDnsUpdater
{
    partial class MainForm
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.infoTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnGetIP = new System.Windows.Forms.Button();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.statusImg = new System.Windows.Forms.CheckBox();
            this.btnUpdateNow = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnEditOK = new System.Windows.Forms.Button();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnResolveHostname = new System.Windows.Forms.Button();
            this.txtIpResolved = new System.Windows.Forms.TextBox();
            this.txtHostResolve = new System.Windows.Forms.TextBox();
            this.chkLog = new System.Windows.Forms.CheckBox();
            this.optAutorunUser = new System.Windows.Forms.RadioButton();
            this.optAutorunAdmin = new System.Windows.Forms.RadioButton();
            this.optAutorunNo = new System.Windows.Forms.RadioButton();
            this.txtHostname = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtUpdateLink = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tryIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnOpenLog = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tryIconMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 315);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(430, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(42, 17);
            this.toolStripStatusLabel1.Text = "Status:";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // trayIcon
            // 
            this.trayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.trayIcon.BalloonTipText = "I\'m here!!!";
            this.trayIcon.BalloonTipTitle = "Info";
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "Dynamic Dns Updater";
            this.trayIcon.Click += new System.EventHandler(this.tryIcon_Click);
            this.trayIcon.DoubleClick += new System.EventHandler(this.tryIcon_DoubleClick);
            // 
            // infoTip
            // 
            this.infoTip.AutoPopDelay = 10000;
            this.infoTip.InitialDelay = 500;
            this.infoTip.ReshowDelay = 100;
            // 
            // btnGetIP
            // 
            this.btnGetIP.Location = new System.Drawing.Point(6, 22);
            this.btnGetIP.Name = "btnGetIP";
            this.btnGetIP.Size = new System.Drawing.Size(92, 27);
            this.btnGetIP.TabIndex = 7;
            this.btnGetIP.Text = "&Get Current IP";
            this.infoTip.SetToolTip(this.btnGetIP, "Obtains your IP address in case you need it");
            this.btnGetIP.UseVisualStyleBackColor = true;
            this.btnGetIP.Click += new System.EventHandler(this.btnGetIP_Click);
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(104, 26);
            this.txtIP.Name = "txtIP";
            this.txtIP.ReadOnly = true;
            this.txtIP.Size = new System.Drawing.Size(92, 20);
            this.txtIP.TabIndex = 8;
            this.infoTip.SetToolTip(this.txtIP, "Here you can find your IP address if you requested it");
            this.txtIP.Click += new System.EventHandler(this.txtIP_Click);
            // 
            // statusImg
            // 
            this.statusImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.statusImg.Appearance = System.Windows.Forms.Appearance.Button;
            this.statusImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.statusImg.Enabled = false;
            this.statusImg.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.statusImg.Location = new System.Drawing.Point(408, 315);
            this.statusImg.Name = "statusImg";
            this.statusImg.Size = new System.Drawing.Size(22, 22);
            this.statusImg.TabIndex = 22;
            this.statusImg.TabStop = false;
            this.statusImg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.infoTip.SetToolTip(this.statusImg, "Shows if the Dynamic DNS update task is running");
            this.statusImg.UseVisualStyleBackColor = false;
            // 
            // btnUpdateNow
            // 
            this.btnUpdateNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateNow.Location = new System.Drawing.Point(221, 22);
            this.btnUpdateNow.Name = "btnUpdateNow";
            this.btnUpdateNow.Size = new System.Drawing.Size(177, 27);
            this.btnUpdateNow.TabIndex = 9;
            this.btnUpdateNow.Text = "&Manually update now your ddns";
            this.infoTip.SetToolTip(this.btnUpdateNow, "Force an update now using the above settings");
            this.btnUpdateNow.UseVisualStyleBackColor = true;
            this.btnUpdateNow.Click += new System.EventHandler(this.btnUpdateNow_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(177, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "min";
            this.infoTip.SetToolTip(this.label1, "How often the program will try to update your dynamic dns record (in minutes)");
            // 
            // btnEditOK
            // 
            this.btnEditOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditOK.Location = new System.Drawing.Point(322, 116);
            this.btnEditOK.Name = "btnEditOK";
            this.btnEditOK.Size = new System.Drawing.Size(78, 27);
            this.btnEditOK.TabIndex = 6;
            this.btnEditOK.Text = "&Edit";
            this.infoTip.SetToolTip(this.btnEditOK, "Saves settings and (re)starts the updater OR stops the updater and edit settings");
            this.btnEditOK.UseVisualStyleBackColor = true;
            this.btnEditOK.Click += new System.EventHandler(this.btnEditOK_Click);
            // 
            // numInterval
            // 
            this.numInterval.Enabled = false;
            this.numInterval.Location = new System.Drawing.Point(85, 116);
            this.numInterval.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.numInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(86, 20);
            this.numInterval.TabIndex = 5;
            this.infoTip.SetToolTip(this.numInterval, "How often the program will try to update your dynamic dns record (in minutes)");
            this.numInterval.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(85, 38);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(315, 20);
            this.txtPassword.TabIndex = 2;
            this.infoTip.SetToolTip(this.txtPassword, "The password that you selected / that has bend send to you by email at the regist" +
        "ration time");
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUser
            // 
            this.txtUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUser.Enabled = false;
            this.txtUser.Location = new System.Drawing.Point(85, 13);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(315, 20);
            this.txtUser.TabIndex = 1;
            this.infoTip.SetToolTip(this.txtUser, "The Username that you used when you registered your new account");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 32;
            this.label5.Text = "Update delay:";
            this.infoTip.SetToolTip(this.label5, "How often the program will try to update your dynamic dns record (in minutes)");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 30;
            this.label4.Text = "Password:";
            this.infoTip.SetToolTip(this.label4, "The password that you selected / that has bend send to you by email at the regist" +
        "ration time");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 28;
            this.label3.Text = "Username:";
            this.infoTip.SetToolTip(this.label3, "The Username that you used when you registered your new account");
            // 
            // btnResolveHostname
            // 
            this.btnResolveHostname.Location = new System.Drawing.Point(6, 64);
            this.btnResolveHostname.Name = "btnResolveHostname";
            this.btnResolveHostname.Size = new System.Drawing.Size(92, 27);
            this.btnResolveHostname.TabIndex = 10;
            this.btnResolveHostname.Text = "&Resolve";
            this.infoTip.SetToolTip(this.btnResolveHostname, "Uses your system dns to resolve a hostname in case you need it");
            this.btnResolveHostname.UseVisualStyleBackColor = true;
            this.btnResolveHostname.Click += new System.EventHandler(this.btnResolveHostname_Click);
            // 
            // txtIpResolved
            // 
            this.txtIpResolved.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIpResolved.Location = new System.Drawing.Point(306, 68);
            this.txtIpResolved.Name = "txtIpResolved";
            this.txtIpResolved.ReadOnly = true;
            this.txtIpResolved.Size = new System.Drawing.Size(92, 20);
            this.txtIpResolved.TabIndex = 12;
            this.infoTip.SetToolTip(this.txtIpResolved, "Here you can find the resolved IP address");
            this.txtIpResolved.Click += new System.EventHandler(this.txtIpResolved_Click);
            // 
            // txtHostResolve
            // 
            this.txtHostResolve.Location = new System.Drawing.Point(104, 68);
            this.txtHostResolve.Name = "txtHostResolve";
            this.txtHostResolve.Size = new System.Drawing.Size(196, 20);
            this.txtHostResolve.TabIndex = 11;
            this.infoTip.SetToolTip(this.txtHostResolve, "Hostname to be resolved");
            // 
            // chkLog
            // 
            this.chkLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkLog.AutoSize = true;
            this.chkLog.Location = new System.Drawing.Point(306, 94);
            this.chkLog.Name = "chkLog";
            this.chkLog.Size = new System.Drawing.Size(92, 17);
            this.chkLog.TabIndex = 16;
            this.chkLog.Text = "Enable log file";
            this.infoTip.SetToolTip(this.chkLog, "Enable the creation of a logfile; logfile is here: %TEMP%\\ddu.log");
            this.chkLog.UseVisualStyleBackColor = true;
            this.chkLog.Click += new System.EventHandler(this.chkLog_Click);
            // 
            // optAutorunUser
            // 
            this.optAutorunUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.optAutorunUser.AutoSize = true;
            this.optAutorunUser.Location = new System.Drawing.Point(9, 114);
            this.optAutorunUser.Name = "optAutorunUser";
            this.optAutorunUser.Size = new System.Drawing.Size(123, 17);
            this.optAutorunUser.TabIndex = 13;
            this.optAutorunUser.Text = "On current user login";
            this.infoTip.SetToolTip(this.optAutorunUser, "Autostart the program with windows; no need to be Admin to use this option (runs " +
        "in backbround+log) (expert note: uses hkcu\\...\\run regedit key)");
            this.optAutorunUser.UseVisualStyleBackColor = true;
            // 
            // optAutorunAdmin
            // 
            this.optAutorunAdmin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.optAutorunAdmin.AutoSize = true;
            this.optAutorunAdmin.Location = new System.Drawing.Point(138, 114);
            this.optAutorunAdmin.Name = "optAutorunAdmin";
            this.optAutorunAdmin.Size = new System.Drawing.Size(107, 17);
            this.optAutorunAdmin.TabIndex = 14;
            this.optAutorunAdmin.Text = "On windows boot";
            this.infoTip.SetToolTip(this.optAutorunAdmin, "Autostart the program with windows; must run the program as Admin to use this opt" +
        "ion (runs in backbround+log) (expert note: uses task scheduler)");
            this.optAutorunAdmin.UseVisualStyleBackColor = true;
            // 
            // optAutorunNo
            // 
            this.optAutorunNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.optAutorunNo.AutoSize = true;
            this.optAutorunNo.Checked = true;
            this.optAutorunNo.Location = new System.Drawing.Point(251, 114);
            this.optAutorunNo.Name = "optAutorunNo";
            this.optAutorunNo.Size = new System.Drawing.Size(39, 17);
            this.optAutorunNo.TabIndex = 15;
            this.optAutorunNo.TabStop = true;
            this.optAutorunNo.Text = "No";
            this.infoTip.SetToolTip(this.optAutorunNo, "Disable both autorun");
            this.optAutorunNo.UseVisualStyleBackColor = true;
            // 
            // txtHostname
            // 
            this.txtHostname.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHostname.Enabled = false;
            this.txtHostname.Location = new System.Drawing.Point(85, 64);
            this.txtHostname.Name = "txtHostname";
            this.txtHostname.Size = new System.Drawing.Size(315, 20);
            this.txtHostname.TabIndex = 3;
            this.infoTip.SetToolTip(this.txtHostname, "Hostname to be updated; for example \"something.dedyn.io\"");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 67);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 37;
            this.label10.Text = "Hostname:";
            this.infoTip.SetToolTip(this.label10, "Hostname to be updated; for example \"something.dedyn.io\"");
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 93);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 13);
            this.label11.TabIndex = 38;
            this.label11.Text = "Update url:";
            this.infoTip.SetToolTip(this.label11, "Url that will be visited to update the ddns, the hostname parameter is automatica" +
        "lly added at the end, https is forced");
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(85, 93);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 13);
            this.label12.TabIndex = 39;
            this.label12.Text = "https://";
            this.infoTip.SetToolTip(this.label12, "HTTPS is forced on");
            // 
            // txtUpdateLink
            // 
            this.txtUpdateLink.Enabled = false;
            this.txtUpdateLink.FormattingEnabled = true;
            this.txtUpdateLink.Items.AddRange(new object[] {
            "update.dedyn.io",
            "dynupdate.no-ip.com/nic/update"});
            this.txtUpdateLink.Location = new System.Drawing.Point(126, 90);
            this.txtUpdateLink.Name = "txtUpdateLink";
            this.txtUpdateLink.Size = new System.Drawing.Size(201, 21);
            this.txtUpdateLink.TabIndex = 40;
            this.txtUpdateLink.Text = "update.dedyn.io";
            this.infoTip.SetToolTip(this.txtUpdateLink, "Url that will be visited to update the ddns, the hostname parameter is automatica" +
        "lly added at the end, https is forced");
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(326, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "?hostname=...";
            this.infoTip.SetToolTip(this.label6, "Hostname parameter is automatically added at the end");
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtUpdateLink);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtHostname);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnEditOK);
            this.groupBox1.Controls.Add(this.numInterval);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.txtUser);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(406, 150);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Main settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnOpenLog);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.optAutorunNo);
            this.groupBox2.Controls.Add(this.optAutorunAdmin);
            this.groupBox2.Controls.Add(this.optAutorunUser);
            this.groupBox2.Controls.Add(this.chkLog);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtHostResolve);
            this.groupBox2.Controls.Add(this.btnResolveHostname);
            this.groupBox2.Controls.Add(this.txtIpResolved);
            this.groupBox2.Controls.Add(this.btnGetIP);
            this.groupBox2.Controls.Add(this.txtIP);
            this.groupBox2.Controls.Add(this.btnUpdateNow);
            this.groupBox2.Location = new System.Drawing.Point(12, 168);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(406, 141);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Utility";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 98);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 39;
            this.label9.Text = "Autorun:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(104, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "Your IP";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(306, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(104, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(192, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Hostname (example: duckduckgo.com)";
            // 
            // tryIconMenu
            // 
            this.tryIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.tryIconMenu.Name = "tryIconMenu";
            this.tryIconMenu.Size = new System.Drawing.Size(93, 26);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // btnOpenLog
            // 
            this.btnOpenLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenLog.Location = new System.Drawing.Point(306, 112);
            this.btnOpenLog.Name = "btnOpenLog";
            this.btnOpenLog.Size = new System.Drawing.Size(75, 23);
            this.btnOpenLog.TabIndex = 40;
            this.btnOpenLog.Text = "&View Log";
            this.infoTip.SetToolTip(this.btnOpenLog, "Opens the logfile if exists");
            this.btnOpenLog.UseVisualStyleBackColor = true;
            this.btnOpenLog.Click += new System.EventHandler(this.btnOpenLog_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 337);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusImg);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nesos - Dynamic Dns Updater";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tryIconMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.ToolTip infoTip;
        private System.Windows.Forms.CheckBox statusImg;
        private System.Windows.Forms.Button btnGetIP;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Button btnUpdateNow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnEditOK;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numInterval;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtHostResolve;
        private System.Windows.Forms.Button btnResolveHostname;
        private System.Windows.Forms.TextBox txtIpResolved;
        private System.Windows.Forms.ContextMenuStrip tryIconMenu;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkLog;
        private System.Windows.Forms.RadioButton optAutorunNo;
        private System.Windows.Forms.RadioButton optAutorunAdmin;
        private System.Windows.Forms.RadioButton optAutorunUser;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtHostname;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox txtUpdateLink;
        private System.Windows.Forms.Button btnOpenLog;
    }
}

