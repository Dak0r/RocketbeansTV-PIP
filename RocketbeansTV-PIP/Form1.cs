using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Dynamic;
using System.Web.Helpers;
using RocketbeansPIP.Properties;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace RocketbeansPIP
{
    public partial class Form1 : Form, IMessageFilter
    {
        private static Form1 instance;
        public const float  CURRENT_VERSION = 3.0f;

        // Used for custom scaling method:
        private Point mouseposition;
        private Size mouseScale;
        private Point lastMouse;

        // Used for hiding UI and show schedule
        private bool activated = true;
        private bool toolbarHidden = false;
        private FormSendeplan formSendeplan;


        // Twitter isc currently not supported.
        private int chatState = 0; // 0 = Disabled, 1 = Twitch, 2 = Twitter
        private int chatSizeState = 0; // 0 = Disabled, 1 = Twitch, 2 = Twitter

        private int extraWidth = 300; //extra width used for calulating the perfect aspect ratio fot the stream, even with the chat displayed
        private int etcPanelBaseWidth = 33;

        private int ExtraWidth
        {
            get { return extraWidth; }
            set
            {
                extraWidth = value;
                etcPanel.Size = new Size(etcPanelBaseWidth + extraWidth, 27);

                Size locSize = this.Size - etcPanel.Size;
                Point location = new Point(locSize.Width, locSize.Height);
                etcPanel.Location = location;
            }
        }

        //private Point browserInitPosition;  // Was used for an early version for twitter support, using twitters api is probably better. The browser had to be moved so that the twitter feed was fully shown in the small window

        public Form1()
        {
            instance = this;
            InitializeComponent();

            // Register Handlers to detect mouse over, mouse leaving, mouse entering etc!
            // The Forms handlers don't detect mouse events in children
            btnMove.MouseMove += new MouseEventHandler(MainFormMouseMove);
            btnMove.MouseDown += new MouseEventHandler(MainFormMouseDown);
            btnScale.MouseMove += new MouseEventHandler(MainFormMouseScale);
            btnScale.MouseDown += new MouseEventHandler(MainFormMouseDown);
            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            Application.AddMessageFilter(this);

            // Create Schedule
            formSendeplan = new FormSendeplan();
            this.AddOwnedForm(formSendeplan);



            // TWITCH PLAYERS

            // standard:
            // axShockwaveFlash1.Movie = "http://www-cdn.jtvnw.net/swflibs/TwitchPlayer.swf";
            // axShockwaveFlash1.FlashVars = "id=ember860-flash-player&hide_chat=true&channel=rocketbeanstv&embed=0&auto_play=true&device_id=6e2bb194806590dc&test_environment_url=http://www.twitch.tv&eventsCallback=Twitch.player.FlashPlayer2.callbacks.callback0";

            // this one doesnt't show the twitch logo:
            axShockwaveFlash1.Movie = "http://www-cdn.jtvnw.net/swflibs/TwitchPlayer.swf?channel=rocketbeanstv&playerType=facebook";

            // register boss key!
            setHotKey(KeyModifiers.Alt, Keys.Y);

            // Display current verison and kick off Version Check
            lblVersion.Text = "Version: " + CURRENT_VERSION;
            tmrAsyncLoading.Start();


            //Set perfect ratio
            Size tmpSize = this.Size;
            tmpSize = (Size)RocketbeansPIP.Properties.Settings.Default["Size"]; // Load last window size
            tmpSize.Width = (int)((tmpSize.Height) * 1.7778f); // 1.777778f = 16/9, probalby causes rounding errors
            tmpSize.Width += ExtraWidth;                        //add ChatSize (ExtraWidth) to perfect aspect ratio
            this.Size = tmpSize;
            axShockwaveFlash1.Width = Size.Width - ExtraWidth;  //set size of flash object without ChatSize (ExtraWidth)

            this.Location = (Point)Properties.Settings.Default["Location"]; //Load last window location
            ChatState = (int)Properties.Settings.Default["ChatState"]; //Load last chat state

            // We need the display resultion, so that the schedule will never open outside of the screen
            Screen screen = GetCurrentScreen();
            if (screen == null)
            {
                this.Location = new Point(640, 320);
            }
        }


        private int ChatState
        {
            get {
                return chatState;
            }
            set
            {
                chatState = value;
                if (chatState == 0) // No chat
                {
                    //webBrowser1.Width = 300; //This moved the browser "back" in that twitter test
                    //webBrowser1.Location = browserInitPosition; 
                    webBrowserChat.Url = null;
                    webBrowserChat.Hide();
                    ExtraWidth = 0;
                    btnTwitchChat.BackgroundImage = Resources.iconChatDisabled;
                }
                else if (chatState == 1) // Twitch Chat!
                {
                    // webBrowser1.Width = 300;
                    // webBrowser1.Location = browserInitPosition;
                    webBrowserChat.Show();
                    ExtraWidth = 300;
                    webBrowserChat.Url = new Uri("http://www.twitch.tv/rocketbeanstv/chat?popout=");
                    btnTwitchChat.BackgroundImage = Resources.iconChatTwitch;
                    webBrowserChat.ScrollBarsEnabled = false;
                    webBrowserChat.IsWebBrowserContextMenuEnabled = false;
                }
                else if (chatState == 2) // Twitter feed.. caused some trouble so it's not activated
                {
                    webBrowserChat.Width = 550;
                    // Point adjPosition = browserInitPosition;
                    // adjPosition.X -= 250;
                    //  webBrowser1.Location = adjPosition;
                    // webBrowser1.DocumentText = "<html><head></head><body><a class=\"twitter-timeline\" data-dnt=\"true\" href=\"https://twitter.com/hashtag/rbtv\" data-widget-id=\"585402306046353408\">#rbtv-Tweets</a> <script>!function(d,s,id){var js,fjs=d.getElementsByTagName(s)[0],p=/^http:/.test(d.location)?'http':'https';if(!d.getElementById(id)){js=d.createElement(s);js.id=id;js.src=p+\"://platform.twitter.com/widgets.js\";fjs.parentNode.insertBefore(js,fjs);}}(document,\"script\",\"twitter-wjs\");</script></body></html> ";
                    webBrowserChat.Show();
                    webBrowserChat.Url = new Uri("http://mobile.twitter.com/hashtag/%23rbtv?f=realtime&src=hash");
                    ExtraWidth = 550;
                    btnTwitchChat.BackgroundImage = Resources.iconChatTwitter;
                    webBrowserChat.ScrollBarsEnabled = true;
                    webBrowserChat.IsWebBrowserContextMenuEnabled = true;
                }
                UpdateSize();
            }
        }

        public bool HideToolbar
        {
            get { return toolbarHidden; }
            set
            {
                if (toolbarHidden != value)
                {
                    toolbarHidden = value;
                    if (toolbarHidden)
                    {
                        axShockwaveFlash1.Height += 31;
                        if (chatState == 1)
                        {
                            // webBrowser1.Height += 135;

                        }
                        else if (chatState == 2)
                        {
                            // webBrowser1.Height += 150;
                        }
                        chatSizeState = chatState;
                    }
                    else
                    {
                        axShockwaveFlash1.Height -= 31;
                        if (chatSizeState == 1)
                        {
                            //   webBrowser1.Height -= 135;
                        }
                        else if (chatState == 2)
                        {
                            // webBrowser1.Height -= 150;
                        }
                    }
                }
            }
        }


        private void UpdateSize()
        {
            if (WindowState != FormWindowState.Maximized)
            {
                Size tmpSize = this.Size;
                tmpSize.Width = (int)((tmpSize.Height) * 1.7778f);  // 1.777778f = 16/9, probalby causes rounding errors
                tmpSize.Width += ExtraWidth;
                this.Size = tmpSize;
            }
            axShockwaveFlash1.Width = Size.Width - ExtraWidth;
        }

        private void btnTwitchChat_Click(object sender, EventArgs e)
        {

            int newchatState = (chatState + 1) % 2; // change modulo to 3 to reactiavte twitter (twitter doesn't wokr currently)

            ChatState = newchatState;

            RocketbeansPIP.Properties.Settings.Default["ChatState"] = chatState;
            RocketbeansPIP.Properties.Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.resetTopMost();
        }

        void Form1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None; // Hide from Taskbar when BossKey is pressed
        }

        private void tmrGUI_Tick(object sender, EventArgs e)
        {
            if (menuPanel.Visible)
            {
                menuPanel.Hide();
                Cursor.Hide();
                Debug.WriteLine("Hide GUI after Time!");
            }

        }

        private void Form1_Activated(object sender, EventArgs e)
        {
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Process.Start("http://rocketbeans.tv/");
        }

        private void btnRBTVLogo_Click(object sender, EventArgs e)
        {
            Process.Start("http://rocketbeans.tv/");
        }

        private void labelCredit_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.danielkorgel.com");
        }

        private void btnMaximieren_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
                tmrGUI.Start();
                axShockwaveFlash1.Width = this.Size.Width - ExtraWidth;
                btnScale.Hide();
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                tmrGUI.Stop();
                Cursor.Show();
                axShockwaveFlash1.Width = this.Size.Width - ExtraWidth;
                btnScale.Show();

                UpdateSize();
            }

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            unSetHotKey();
        }

        // If menuPanel changes visivility, change visbility of complete Toolbar.
        private void menuPanel_VisibleChanged(object sender, EventArgs e)
        {
            etcPanel.Visible = menuPanel.Visible;
            HideToolbar = !menuPanel.Visible;
        }

        #region Display Schedule
        private void btnSendeplan_MouseHover(object sender, EventArgs e)
        {
            if (!formSendeplan.Visible)
            {
                this.TopMost = false;
                tmrTopMost.Enabled = false;
                formSendeplan.Show();
                Point mouse = MousePosition;

                // The following code takes care, that schedule is never shown outside of the current screen!

                //Determine current Screen
                Screen screen = GetCurrentScreen();
                if (screen == null)
                {
                    screen = Screen.PrimaryScreen;
                }

                //Calculate it should be shown left or right of the mouse position
                if (mouse.X + formSendeplan.Width > screen.Bounds.Right)
                {
                    mouse.X -= formSendeplan.Width - 100;
                }
                else
                {
                    mouse.X -= 200;
                }

                //Calculate it should be shown above or beneath of the mouse positionn
                if (mouse.Y + formSendeplan.Height > screen.Bounds.Bottom)
                {
                    mouse.Y -= formSendeplan.Height + 10;
                }
                else
                {
                    mouse.Y += 10;
                }


                formSendeplan.Location = mouse;
                formSendeplan.TopMost = true;
            }
        }



        private void btnSendeplan_MouseLeave(object sender, EventArgs e)
        {
            if (formSendeplan.Visible)
            {
                formSendeplan.Hide();
                formSendeplan.TopMost = false;
            }
            tmrTopMost.Enabled = false;
            resetTopMost();
        }

        private Screen GetCurrentScreen()
        {
            Screen screen = null;
            foreach (Screen s in Screen.AllScreens)
            {
                if (Location.X > s.Bounds.Left && Location.X < s.Bounds.Right && Location.Y > s.Bounds.Top && Location.Y < s.Bounds.Bottom)
                {
                    screen = s;
                    break;
                }
            }
            return screen;
        }

        private void btnSendeplan_Click(object sender, EventArgs e)
        {
            //was used to open the google calender, but it turned out to be too annyoing
            /*if (formSendeplan.Visible) 
            { 
                Process.Start("https://www.google.com/calendar/embed?src=h6tfehdpu3jrbcrn9sdju9ohj8%40group.calendar.google.com&ctz=Europe/Berlin");
            }*/
        }

        #endregion

        #region Keep On Top
        public void resetTopMost() // Keep Form on top of everything! Needs to be reseted every now and then, in case other applications also use "TopMost".
        {
            this.TopMost = false;
            this.TopMost = true;
        }

        private void tmrTopMost_Tick(object sender, EventArgs e)
        {
            this.resetTopMost();
        }
        #endregion

        #region Window Moving and Scaling
        //used by MainFormMouseMove and MainFormMouseScale to get mouse position on click
        private void MainFormMouseDown(object sender, MouseEventArgs e)
        {
            mouseposition = new Point(-e.X, -e.Y);
            lastMouse = new Point(-Control.MousePosition.X, -Control.MousePosition.Y);
            mouseScale = this.Size;
        }
        private void MainFormMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(mouseposition.X, mouseposition.Y);
                this.Location = mousePos;

                RocketbeansPIP.Properties.Settings.Default["Location"] = this.Location;
                RocketbeansPIP.Properties.Settings.Default.Save();
            }
        }
        private void MainFormMouseScale(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.WindowState == FormWindowState.Maximized)
                {
                    return;
                }
                Point mousePos = Control.MousePosition;
                mousePos.Offset(lastMouse.X, lastMouse.Y);

                Size tmpSize = this.Size;
                Size oldSize = this.Size;
                tmpSize.Height = mouseScale.Height + mousePos.Y;
                tmpSize.Width = mouseScale.Width + mousePos.X;

                axShockwaveFlash1.Width = this.Size.Width - ExtraWidth;

                if (tmpSize.Width - ExtraWidth < 270)
                {
                    tmpSize.Width = this.Size.Width;
                }
                if (tmpSize.Height < 180)
                {
                    tmpSize.Height = this.Size.Height;
                }

                Size delta = tmpSize - oldSize;

                //if (Math.Abs(delta.Width) > Math.Abs(delta.Height))
                if (Math.Abs(mousePos.X) > Math.Abs(mousePos.Y))
                {
                    //change in Width is bigger
                    //                    tmpSize.Height = (int) (tmpSize.Width * 0.5625f) +31;
                    tmpSize.Height = (int)((tmpSize.Width - ExtraWidth) * 0.5625f);
                }
                else
                {
                    //Change in Height is bigger
                    //tmpSize.Width = (int) ((tmpSize.Height-31) * 1.7778f);
                    tmpSize.Width = (int)((tmpSize.Height) * 1.7778f) + ExtraWidth;
                }

                tmpSize.Width -= ExtraWidth;

                RocketbeansPIP.Properties.Settings.Default["Size"] = this.Size;
                tmpSize.Width += ExtraWidth;
                this.Size = tmpSize;
                RocketbeansPIP.Properties.Settings.Default.Save();
                //lastMouse = mousePos;
            }
        }
        #endregion

        #region Custom On Mouse Leave and Enter Window Handler

        private void Form1_MouseLeave_1(object sender, EventArgs e)
        {

            if (!(MousePosition.X >= this.Left && MousePosition.X <= this.Right && MousePosition.Y >= this.Top && MousePosition.Y <= this.Bottom))
            {
                if (menuPanel.Visible)
                {
                    if (activated)
                    {
                        menuPanel.Hide();
                    }
                }
            }

        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            if (!menuPanel.Visible)
            {
                menuPanel.Show();
            }
        }

        const int WM_MOUSELEAVE = 0x02A3;//&H2A3;
        const int WM_MOUSEMOVE = 0x0200;//&H200;
        const int WM_MOUSEHOVER = 0x02A1;
        private Point lastMousePos = MousePosition;
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_MOUSELEAVE || m.Msg == WM_MOUSEMOVE)
            {
                if (m.Msg == WM_MOUSELEAVE)
                {
                    bool hit = menuPanel.ClientRectangle.Contains(menuPanel.PointToClient(MousePosition));
                    if (!hit)
                    {
                        hit = etcPanel.ClientRectangle.Contains(etcPanel.PointToClient(MousePosition));
                        if (!hit)
                        {
                            this.Form1_MouseLeave_1(null, null);
                            //  System.Diagnostics.Debug.WriteLine("CustomLeave");
                            return false;
                        }
                    }
                }
                else
                {

                    if (!menuPanel.Visible)
                    {
                        if (this.WindowState != FormWindowState.Maximized)
                        {
                            this.Form1_MouseEnter(null, null);
                        }
                        else if (MousePosition != lastMousePos)
                        {
                            this.Form1_MouseEnter(null, null);
                            tmrGUI.Stop();
                            tmrGUI.Start();
                            Cursor.Show();
                            Debug.WriteLine("Timer Reset!");
                            lastMousePos = MousePosition;
                        }
                    }
                    //  System.Diagnostics.Debug.WriteLine("CustoMove");
                }
                return false;
            }
            return false;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowserChat.Document.AttachEventHandler("onmouseout", BrwoserMouseLeave);
        }

        private void BrwoserMouseLeave(object sender, EventArgs e)
        {
            this.Form1_MouseLeave_1(null, null);
        }


        #endregion

        #region Update Check
        private void lblVersion_Click(object sender, EventArgs e)
        {
            Process.Start("http://forum.rocketbeans.tv/c/project/development");
        }
        private void tmrAsyncLoading_Tick(object sender, EventArgs e)
        {
            tmrAsyncLoading.Stop();
            tmrAsyncLoading.Enabled = false;
            CheckForUpdates();

        }
        public void CheckForUpdates()
        {
            lblVersion.Text = "Version: " + CURRENT_VERSION + " (Prüfe auf Updates)";
            DownloadFileFromDropbox("https://www.dropbox.com/s/tiaz217ftqsocki/currentVersion.txt?dl=1");

        }

        public static void CompareVersion(String newVersion)
        {

            float newestVersion = -1;
            float lastNotifiedVersion = (float)RocketbeansPIP.Properties.Settings.Default["LastNotifiedVersion"];
            instance.lblVersion.Text = "Version: " + CURRENT_VERSION;

            if (newVersion == "") { newVersion = "-1"; }
            try
            {
                newestVersion = float.Parse(newVersion);
            }
            catch (Exception)
            {
                newestVersion = -1;
            }
            if (CURRENT_VERSION < newestVersion)
            {
                instance.lblVersion.Text += " !! UPDATE VERFÜGBAR !!";
                if (lastNotifiedVersion < newestVersion)
                {
                    RocketbeansPIP.Properties.Settings.Default["LastNotifiedVersion"] = newestVersion;
                    RocketbeansPIP.Properties.Settings.Default.Save();
                    MessageBox.Show("Eine neue Version ist verfügbar! Neuste Version " + newestVersion + Environment.NewLine + "Besuche das Rocketbeans Forum (http://forum.rocketbeans.tv/) um die neuste Version zu erhalten! (Oder klicke auf die Versions Nummer in der Anwendung)", "RocketbeansPIP - Update Verfügbar");

                }
            }
        }

        public static string DownloadFileFromDropbox(string dropboxUrl)
        {
            string content = "";
            WebClient webclient = null;
            try
            {
                webclient = new WebClient();

                RemoteCertificateValidationCallback old = ServicePointManager.ServerCertificateValidationCallback;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidRemoteCertificate);
                webclient.DownloadStringCompleted += DownloadStringCompleted;
                webclient.DownloadStringAsync(new Uri(dropboxUrl));
                ServicePointManager.ServerCertificateValidationCallback = old;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (webclient != null)
                {
                    // webclient.Dispose();
                    //  webclient = null;
                }
            }
            return content;
        }
        public static void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //byte[] raw = e.Result;
            if (e.Error == null)
            {
                CompareVersion(e.Result);
            }
            else
            {
                instance.lblVersion.Text = "Version: " + CURRENT_VERSION + " [O]";
            }
            ((WebClient)sender).Dispose();

        }

        private static bool ValidRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (certificate.Subject.Contains("dropboxusercontent.com"))
            {
                return true;
            }
            else if (certificate.Subject.Contains("dropbox.com"))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Boss Key Feature
        //Hot Key stuff, used for Boss Key
        //API Imports
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
        IntPtr hWnd, // handle to window
        int id, // hot key identifier
        KeyModifiers fsModifiers, // key-modifier options
        Keys vk // virtual-key code 
        );
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
        IntPtr hWnd, // handle to window
        int id // hot key identifier
        );
        const int HOTKEY_ID = 31197; //Any number to use to identify the hotkey instance
        public enum KeyModifiers //enum to call 3rd parameter of RegisterHotKey easily
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }
        public bool setHotKey(KeyModifiers Kmds, Keys key)
        {
            return RegisterHotKey(this.Handle, HOTKEY_ID, Kmds, key);
        }
        public bool unSetHotKey()
        {
            return UnregisterHotKey(this.Handle, HOTKEY_ID);
        }

        const int WM_HOTKEY = 0x0312;// hotkey message identifier
        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case WM_HOTKEY:
                    if (this.Visible)
                    {
                        this.Hide();
                    }
                    else
                    {
                        this.Show();
                    }
                    break;
            }
            base.WndProc(ref message);
        }
        #endregion
    }


}
