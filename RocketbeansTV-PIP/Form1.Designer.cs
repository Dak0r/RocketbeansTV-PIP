using System.Drawing;
namespace RocketbeansPIP
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tmrTopMost = new System.Windows.Forms.Timer(this.components);
            this.tmrGUI = new System.Windows.Forms.Timer(this.components);
            this.etcPanel = new System.Windows.Forms.Panel();
            this.btnScale = new System.Windows.Forms.Button();
            this.menuPanel = new System.Windows.Forms.Panel();
            this.btnMove = new System.Windows.Forms.Button();
            this.btnTwitchChat = new System.Windows.Forms.Button();
            this.btnSendeplan = new System.Windows.Forms.Button();
            this.btnMaximieren = new System.Windows.Forms.Button();
            this.btnRBTVLogo = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblCredit = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.tmrAsyncLoading = new System.Windows.Forms.Timer(this.components);
            this.webBrowserChat = new System.Windows.Forms.WebBrowser();
            this.webBrowserMovie = new System.Windows.Forms.WebBrowser();
            this.lbl_zuschauer = new System.Windows.Forms.Label();
            this.etcPanel.SuspendLayout();
            this.menuPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrTopMost
            // 
            this.tmrTopMost.Interval = 10000;
            this.tmrTopMost.Tick += new System.EventHandler(this.tmrTopMost_Tick);
            // 
            // tmrGUI
            // 
            this.tmrGUI.Interval = 1000;
            this.tmrGUI.Tick += new System.EventHandler(this.tmrGUI_Tick);
            // 
            // etcPanel
            // 
            this.etcPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.etcPanel.BackColor = System.Drawing.Color.Transparent;
            this.etcPanel.BackgroundImage = global::RocketbeansPIP.Properties.Resources.twitchBG_neu;
            this.etcPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.etcPanel.Controls.Add(this.btnScale);
            this.etcPanel.Location = new System.Drawing.Point(605, 358);
            this.etcPanel.Name = "etcPanel";
            this.etcPanel.Size = new System.Drawing.Size(45, 37);
            this.etcPanel.TabIndex = 5;
            // 
            // btnScale
            // 
            this.btnScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScale.BackColor = System.Drawing.Color.Transparent;
            this.btnScale.BackgroundImage = global::RocketbeansPIP.Properties.Resources.twitchBG_neu;
            this.btnScale.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnScale.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.btnScale.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnScale.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnScale.Image = global::RocketbeansPIP.Properties.Resources.iconScale3;
            this.btnScale.Location = new System.Drawing.Point(0, 0);
            this.btnScale.Name = "btnScale";
            this.btnScale.Size = new System.Drawing.Size(45, 37);
            this.btnScale.TabIndex = 4;
            this.btnScale.UseVisualStyleBackColor = false;
            // 
            // menuPanel
            // 
            this.menuPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.menuPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.menuPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.menuPanel.BackgroundImage = global::RocketbeansPIP.Properties.Resources.twitchBG_neu;
            this.menuPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.menuPanel.Controls.Add(this.lbl_zuschauer);
            this.menuPanel.Controls.Add(this.btnMove);
            this.menuPanel.Controls.Add(this.btnTwitchChat);
            this.menuPanel.Controls.Add(this.btnSendeplan);
            this.menuPanel.Controls.Add(this.btnMaximieren);
            this.menuPanel.Controls.Add(this.btnRBTVLogo);
            this.menuPanel.Controls.Add(this.btnExit);
            this.menuPanel.Controls.Add(this.lblCredit);
            this.menuPanel.Controls.Add(this.lblVersion);
            this.menuPanel.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.menuPanel.Location = new System.Drawing.Point(0, 0);
            this.menuPanel.Name = "menuPanel";
            this.menuPanel.Size = new System.Drawing.Size(650, 57);
            this.menuPanel.TabIndex = 2;
            this.menuPanel.VisibleChanged += new System.EventHandler(this.menuPanel_VisibleChanged);
            // 
            // btnMove
            // 
            this.btnMove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMove.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMove.BackColor = System.Drawing.Color.Transparent;
            this.btnMove.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.btnMove.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnMove.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMove.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnMove.Location = new System.Drawing.Point(55, 3);
            this.btnMove.Name = "btnMove";
            this.btnMove.Size = new System.Drawing.Size(405, 23);
            this.btnMove.TabIndex = 3;
            this.btnMove.Text = "RocketbeansTV PIP";
            this.btnMove.UseVisualStyleBackColor = false;
            this.btnMove.MouseLeave += new System.EventHandler(this.btnMove_MouseLeave);
            this.btnMove.MouseHover += new System.EventHandler(this.btnMove_MouseHover);
            // 
            // btnTwitchChat
            // 
            this.btnTwitchChat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTwitchChat.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnTwitchChat.BackColor = System.Drawing.Color.Transparent;
            this.btnTwitchChat.BackgroundImage = global::RocketbeansPIP.Properties.Resources.iconChatDisabled;
            this.btnTwitchChat.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnTwitchChat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTwitchChat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTwitchChat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnTwitchChat.Location = new System.Drawing.Point(466, 3);
            this.btnTwitchChat.Name = "btnTwitchChat";
            this.btnTwitchChat.Size = new System.Drawing.Size(42, 34);
            this.btnTwitchChat.TabIndex = 10;
            this.btnTwitchChat.UseVisualStyleBackColor = false;
            this.btnTwitchChat.Click += new System.EventHandler(this.btnTwitchChat_Click);
            // 
            // btnSendeplan
            // 
            this.btnSendeplan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendeplan.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSendeplan.BackColor = System.Drawing.Color.Transparent;
            this.btnSendeplan.BackgroundImage = global::RocketbeansPIP.Properties.Resources.iconSendeplan;
            this.btnSendeplan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSendeplan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSendeplan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendeplan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnSendeplan.Location = new System.Drawing.Point(514, 3);
            this.btnSendeplan.Name = "btnSendeplan";
            this.btnSendeplan.Size = new System.Drawing.Size(42, 34);
            this.btnSendeplan.TabIndex = 9;
            this.btnSendeplan.UseVisualStyleBackColor = false;
            this.btnSendeplan.Click += new System.EventHandler(this.btnSendeplan_Click);
            this.btnSendeplan.MouseLeave += new System.EventHandler(this.btnSendeplan_MouseLeave);
            this.btnSendeplan.MouseHover += new System.EventHandler(this.btnSendeplan_MouseHover);
            // 
            // btnMaximieren
            // 
            this.btnMaximieren.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximieren.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnMaximieren.BackColor = System.Drawing.Color.Transparent;
            this.btnMaximieren.BackgroundImage = global::RocketbeansPIP.Properties.Resources.iconMaximizeSmall;
            this.btnMaximieren.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMaximieren.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMaximieren.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximieren.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnMaximieren.Location = new System.Drawing.Point(562, 3);
            this.btnMaximieren.Name = "btnMaximieren";
            this.btnMaximieren.Size = new System.Drawing.Size(42, 34);
            this.btnMaximieren.TabIndex = 8;
            this.btnMaximieren.UseVisualStyleBackColor = false;
            this.btnMaximieren.Click += new System.EventHandler(this.btnMaximieren_Click);
            // 
            // btnRBTVLogo
            // 
            this.btnRBTVLogo.BackColor = System.Drawing.Color.Transparent;
            this.btnRBTVLogo.BackgroundImage = global::RocketbeansPIP.Properties.Resources.rocketbeanstv_profile_image_small;
            this.btnRBTVLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRBTVLogo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRBTVLogo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRBTVLogo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnRBTVLogo.Location = new System.Drawing.Point(3, 0);
            this.btnRBTVLogo.Name = "btnRBTVLogo";
            this.btnRBTVLogo.Size = new System.Drawing.Size(46, 40);
            this.btnRBTVLogo.TabIndex = 6;
            this.btnRBTVLogo.UseVisualStyleBackColor = false;
            this.btnRBTVLogo.Click += new System.EventHandler(this.btnRBTVLogo_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BackgroundImage = global::RocketbeansPIP.Properties.Resources.iconCrossSmall3;
            this.btnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnExit.Location = new System.Drawing.Point(605, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(42, 34);
            this.btnExit.TabIndex = 2;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblCredit
            // 
            this.lblCredit.BackColor = System.Drawing.Color.Transparent;
            this.lblCredit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblCredit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCredit.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lblCredit.Location = new System.Drawing.Point(52, 23);
            this.lblCredit.Name = "lblCredit";
            this.lblCredit.Size = new System.Drawing.Size(108, 17);
            this.lblCredit.TabIndex = 7;
            this.lblCredit.Text = "DanielKorgel.com";
            this.lblCredit.Click += new System.EventHandler(this.labelCredit_Click);
            // 
            // lblVersion
            // 
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lblVersion.Location = new System.Drawing.Point(166, 23);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(282, 17);
            this.lblVersion.TabIndex = 11;
            this.lblVersion.Click += new System.EventHandler(this.lblVersion_Click);
            // 
            // tmrAsyncLoading
            // 
            this.tmrAsyncLoading.Interval = 500;
            this.tmrAsyncLoading.Tick += new System.EventHandler(this.tmrAsyncLoading_Tick);
            // 
            // webBrowserChat
            // 
            this.webBrowserChat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowserChat.Location = new System.Drawing.Point(350, -50);
            this.webBrowserChat.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserChat.Name = "webBrowserChat";
            this.webBrowserChat.ScriptErrorsSuppressed = true;
            this.webBrowserChat.ScrollBarsEnabled = false;
            this.webBrowserChat.Size = new System.Drawing.Size(300, 445);
            this.webBrowserChat.TabIndex = 6;
            this.webBrowserChat.Url = new System.Uri("https://www.youtube.com/live_chat?v=OH_shyI3IGM&is_popout=1", System.UriKind.Absolute);
            this.webBrowserChat.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // webBrowserMovie
            // 
            this.webBrowserMovie.AllowWebBrowserDrop = false;
            this.webBrowserMovie.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.webBrowserMovie.Location = new System.Drawing.Point(0, 0);
            this.webBrowserMovie.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowserMovie.Name = "webBrowserMovie";
            this.webBrowserMovie.ScrollBarsEnabled = false;
            this.webBrowserMovie.Size = new System.Drawing.Size(518, 395);
            this.webBrowserMovie.TabIndex = 7;
            // 
            // lbl_zuschauer
            // 
            this.lbl_zuschauer.BackColor = System.Drawing.Color.Transparent;
            this.lbl_zuschauer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbl_zuschauer.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_zuschauer.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.lbl_zuschauer.Location = new System.Drawing.Point(52, 40);
            this.lbl_zuschauer.Name = "lbl_zuschauer";
            this.lbl_zuschauer.Size = new System.Drawing.Size(108, 17);
            this.lbl_zuschauer.TabIndex = 12;
            this.lbl_zuschauer.Text = "Zuschauer: Lädt.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(650, 395);
            this.Controls.Add(this.etcPanel);
            this.Controls.Add(this.menuPanel);
            this.Controls.Add(this.webBrowserMovie);
            this.Controls.Add(this.webBrowserChat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "RocketbeansTV PIP";
            this.TopMost = true;
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.etcPanel.ResumeLayout(false);
            this.menuPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrTopMost;
        private System.Windows.Forms.Timer tmrGUI;
        private System.Windows.Forms.Panel menuPanel;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnMove;
        private System.Windows.Forms.Button btnScale;
        private System.Windows.Forms.Panel etcPanel;
        private System.Windows.Forms.Button btnRBTVLogo;
        private System.Windows.Forms.Label lblCredit;
        private System.Windows.Forms.Button btnMaximieren;
        private System.Windows.Forms.Button btnSendeplan;
        private System.Windows.Forms.WebBrowser webBrowserChat;
        private System.Windows.Forms.Button btnTwitchChat;
        private System.Windows.Forms.Timer tmrAsyncLoading;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.WebBrowser webBrowserMovie;
        private System.Windows.Forms.Label lbl_zuschauer;
    }
}

