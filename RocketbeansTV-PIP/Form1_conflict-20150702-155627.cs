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

        //HOT KEY STUFF
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

        private int chatState = 0; /* 0 = Disabled, 1 = Twitch, 2 = Twitter */
        private int chatSizeState = 0; /* 0 = Disabled, 1 = Twitch, 2 = Twitter */

        //private Point browserInitPosition;

        private void setChatState(int newState){
            chatState = newState;
            if(chatState == 0){
              //  webBrowser1.Width = 300;
                //webBrowser1.Location = browserInitPosition;
                webBrowser1.Url = null;
                webBrowser1.Hide();
                ExtraWidth = 0;
                btnTwitchChat.BackgroundImage = Resources.iconChatDisabled;
            }else if(chatState == 1){
               // webBrowser1.Width = 300;
               // webBrowser1.Location = browserInitPosition;
                webBrowser1.Show();
                ExtraWidth = 300;
                webBrowser1.Url = new Uri("http://www.twitch.tv/rocketbeanstv/chat?popout=");
                btnTwitchChat.BackgroundImage = Resources.iconChatTwitch;
                webBrowser1.ScrollBarsEnabled = false;
                webBrowser1.IsWebBrowserContextMenuEnabled = false;
            }else if(chatState == 2){
                webBrowser1.Width = 550;
               // Point adjPosition = browserInitPosition;
               // adjPosition.X -= 250;
              //  webBrowser1.Location = adjPosition;
                // webBrowser1.DocumentText = "<html><head></head><body><a class=\"twitter-timeline\" data-dnt=\"true\" href=\"https://twitter.com/hashtag/rbtv\" data-widget-id=\"585402306046353408\">#rbtv-Tweets</a> <script>!function(d,s,id){var js,fjs=d.getElementsByTagName(s)[0],p=/^http:/.test(d.location)?'http':'https';if(!d.getElementById(id)){js=d.createElement(s);js.id=id;js.src=p+\"://platform.twitter.com/widgets.js\";fjs.parentNode.insertBefore(js,fjs);}}(document,\"script\",\"twitter-wjs\");</script></body></html> ";
                webBrowser1.Show();
                webBrowser1.Url = new Uri("http://mobile.twitter.com/hashtag/%23rbtv?f=realtime&src=hash");
                ExtraWidth = 550;
                btnTwitchChat.BackgroundImage = Resources.iconChatTwitter;
                webBrowser1.ScrollBarsEnabled = true;
                webBrowser1.IsWebBrowserContextMenuEnabled = true;
            }
            UpdateSize();
        }
        private void updateChatState()
        {
            setChatState(chatState);
        }


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

        private int ExtraWidth = 300;
        

        const int WM_HOTKEY = 0x0312;//magic hotkey message identifier
        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case WM_HOTKEY:
                    //Keys key = (Keys)(((int)message.LParam >> 16) & 0xFFFF);
                    //KeyModifiers modifier = (KeyModifiers)((int)message.LParam & 0xFFFF);
                    //put your on hotkey code here
                    //MessageBox.Show("HotKey Pressed :" + modifier.ToString() + " " + key.ToString());
                    if (this.Visible)
                    {
                        this.Hide();
                    }
                    else
                    {
                        this.Show();
                    }
                    
                    //end hotkey code
                    break;
            }
            base.WndProc(ref message);
        }
        
        //const string defaultText = "Youtube Link to Video or Playlist (or other flash players)";
        private bool activated = true;
        private bool toolbarHidden = false;
        private FormSendeplan formSendeplan;
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
        public Form1()
        {
            InitializeComponent();

            btnMove.MouseMove += new MouseEventHandler(MainFormMouseMove);
            btnMove.MouseDown += new MouseEventHandler(MainFormMouseDown);
            btnScale.MouseMove += new MouseEventHandler(MainFormMouseScale);
            btnScale.MouseDown += new MouseEventHandler(MainFormMouseDown);


            

            this.MouseMove += new MouseEventHandler(Form1_MouseMove);
            //this.OnMouseLeave += new MouseEventHandler(Form1_MouseMove);

            Application.AddMessageFilter(this);

           // axShockwaveFlash1.AllowFullScreen = "1";
            axShockwaveFlash1.Movie = "http://www-cdn.jtvnw.net/swflibs/TwitchPlayer.swf";
           // axShockwaveFlash1.FlashVars = "id=ember891-flash-player&hide_chat=true&channel=rocketbeanstv&embed=0&amp;auto_play=true&device_id=96389b8253f808ef&eventsCallback=Twitch.player.FlashPlayer2.callbacks.callback0";
            axShockwaveFlash1.FlashVars = "id=ember860-flash-player&hide_chat=true&channel=rocketbeanstv&embed=0&auto_play=true&device_id=6e2bb194806590dc&test_environment_url=http://www.twitch.tv&eventsCallback=Twitch.player.FlashPlayer2.callbacks.callback0";

            setHotKey(KeyModifiers.Alt, Keys.Y);


            //browserInitPosition = webBrowser1.Location;
           

            //Set perfect ratio
            Size tmpSize = this.Size;
            tmpSize = (Size)RocketbeansPIP.Properties.Settings.Default["Size"];
            tmpSize.Width = (int)((tmpSize.Height) * 1.7778f);
            tmpSize.Width += ExtraWidth;
            this.Size = tmpSize;
            axShockwaveFlash1.Width = Size.Width - ExtraWidth;

            this.Location = (Point)Properties.Settings.Default["Location"];

            setChatState(0);
            chatState = (int)Properties.Settings.Default["ChatState"];
            /*if (chatState != 0)
            {
                tmrAsyncLoading.Start();
            }*/
            updateChatState();

            Screen screen = GetCurrentScreen();
            if (screen == null)
            {
                this.Location = new Point(640, 320);
            }


            formSendeplan = new FormSendeplan();
            this.AddOwnedForm(formSendeplan);

            int currentVersion = 2;
            int newestVersion = -1;
            string newVersion = DownloadFileFromDropbox("");
            if (newVersion == "") { newVersion = "-1"; }
            try
            {
                newestVersion = int.Parse(newVersion);
            }
            catch (Exception)
            {
                newestVersion = -1;
            }
            if (currentVersion < newestVersion)
            {
                MessageBox.Show("Eine neue Version ist Verfügbar!");
            }

        }

        private void UpdateSize()
        {
            Size tmpSize = this.Size;
            tmpSize.Width = (int)((tmpSize.Height) * 1.7778f);
            tmpSize.Width += ExtraWidth;
            this.Size = tmpSize;
            axShockwaveFlash1.Width = Size.Width - ExtraWidth;
        }

        private void BrwoserMouseLeave(object sender, EventArgs e)
        {
            this.Form1_MouseLeave_1(null, null);
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            this.resetTopMost();
            //updateSendeplan();
        }

        public void resetTopMost()
        {
            this.TopMost = false;
            this.TopMost = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.resetTopMost();
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            if (!menuPanel.Visible)
            {
                menuPanel.Show();
               // if (timer2.Enabled) { timer2.Enabled = false; }
            }
        }
        void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            // this.FormBorderStyle = FormBorderStyle.Sizable;
            //textBox1.Show();

           
            
          //  if (timer2.Enabled) { timer2.Enabled = false; }
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            //textBox1.Hide();
            //menuPanel.Hide();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (menuPanel.Visible)
            {
                menuPanel.Hide();
                Cursor.Hide();
                Debug.WriteLine("Hide GUI after Time!");
            }

        }

        private void Form1_MouseLeave_1(object sender, EventArgs e)
        {

            if (!(MousePosition.X >= this.Left && MousePosition.X <= this.Right && MousePosition.Y >= this.Top && MousePosition.Y <= this.Bottom))
            {
                if (menuPanel.Visible)
                {
                    //  if (!timer2.Enabled) { timer2.Enabled = true; }
                    if (activated)
                    {
                        menuPanel.Hide();

                    }
                }
            }
            
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            //  this.FormBorderStyle = FormBorderStyle.Sizable;
            //textBox1.Show();
            //menuPanel.Show();
           // if (timer2.Enabled) { timer2.Enabled = false; }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private Point mouseposition;
        private Size mouseScale;
        private Point lastMouse;
        void MainFormMouseDown(object sender, MouseEventArgs e)
        {
            mouseposition = new Point(-e.X, -e.Y);
            lastMouse = new Point(-Control.MousePosition.X, -Control.MousePosition.Y);
            mouseScale = this.Size;
        }
        void MainFormMouseMove(object sender, MouseEventArgs e)
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
        void MainFormMouseScale(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {

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




        private void menuPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            /*if (textBox1.Text.Equals(defaultText))
             {
                 textBox1.Text = "";
             }
             else
             {
                 textBox1.SelectAll();
             }*/
        }

        private void menuPanel_VisibleChanged(object sender, EventArgs e)
        {
           etcPanel.Visible = menuPanel.Visible;
   
           HideToolbar = !menuPanel.Visible;

         
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

        private void label1_Click(object sender, EventArgs e)
        {
            Process.Start("http://rocketbeans.tv/");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("http://rocketbeans.tv/");
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.danielkorgel.de");
        }

        private void axShockwaveFlash1_Enter(object sender, EventArgs e)
        {

        }

        private void btnMove_Click(object sender, EventArgs e)
        {

        }

        private void etcPanel_Paint(object sender, PaintEventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
                tmrGUI.Start();
                axShockwaveFlash1.Width = this.Size.Width - ExtraWidth;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                tmrGUI.Stop();
                Cursor.Show();
                axShockwaveFlash1.Width = this.Size.Width - ExtraWidth;
            }
            
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            unSetHotKey();
        }

        private void tmrSendeplan_Tick(object sender, EventArgs e)
        {
            updateSendeplan();
        }

        public void updateSendeplan()
        {


        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://twitter.com/Telebeans");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.google.com/calendar/embed?src=h6tfehdpu3jrbcrn9sdju9ohj8%40group.calendar.google.com&ctz=Europe/Berlin");
        }

        private void btnSendeplan_MouseHover(object sender, EventArgs e)
        {
            if (!formSendeplan.Visible)
            {
                this.TopMost = false;
                tmrTopMost.Enabled = false;
                formSendeplan.Show();
                Point mouse = MousePosition;
                
                
            

                //Determine current Screen

                Screen screen = GetCurrentScreen();
                if (screen == null)
                {
                    screen = Screen.PrimaryScreen;
                }

                //Calculate left or right
                if (mouse.X + formSendeplan.Width > screen.Bounds.Right)
                {
                    mouse.X -= formSendeplan.Width -100;
                }
                else
                {
                    mouse.X -= 200;
                }

                //Calculate top or bottom
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

        private void btnSendeplan_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.google.com/calendar/embed?src=h6tfehdpu3jrbcrn9sdju9ohj8%40group.calendar.google.com&ctz=Europe/Berlin");
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webBrowser1.Document.AttachEventHandler("onmouseout", BrwoserMouseLeave);
        }

        private void btnTwitchChat_Click(object sender, EventArgs e)
        {

            chatState = (chatState+1) % 2;

            updateChatState();

            RocketbeansPIP.Properties.Settings.Default["ChatState"] = chatState;
            RocketbeansPIP.Properties.Settings.Default.Save();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {

        }

        private void tmrAsyncLoading_Tick(object sender, EventArgs e)
        {
            tmrAsyncLoading.Stop();
            updateChatState();
           
        }


        public static string DownloadFileFromDropbox(string dropboxUrl)
        {
            string content = "";
            WebClient webclient = null;
            try
            {
                webclient = new WebClient();

                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidRemoteCertificate);

               /* if (File.Exists(UpdatesDirStr + file_name))
                {
                    File.Delete(UpdatesDirStr + file_name);
                }
                webclient.DownloadFile(dropboxUrl, UpdatesDirStr + file_name);*/
                content = webclient.DownloadString(dropboxUrl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (webclient != null)
                {
                    webclient.Dispose();
                    webclient = null;
                }
            }
            return content;
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
    }



    /* public static class StringExtension
     {
         public static string ReplaceFirst(this string text, string search, string replace)
         {
             int pos = text.IndexOf(search);
             if (pos < 0)
             {
                 return text;
             }
             return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
         }
     }*/


}
