using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Security.Cryptography;

namespace RocketbeansPIP
{

    public partial class FormSendeplan : Form
    {
// IF YOU ARE JUST HERE FOR THE RBTV CODE, SCROLL DOWN TO THE RBTV SENDEPLAN API REGION (~line 200)
#region UiStuff
        public static FormSendeplan instance;

        private bool useRBTVAPI = true; // if false Google API will be used
        private int minDefferenceMinutes = 15; //The GUI and the "Current Show" marker will be updated every time. But we'll download new json data only if minDefferenceMinutes have passed since the last download
        private DateTime lastDownload;
        private bool downloadedScheduleDataOnce = false;

        private string jsonSendeplan = "";
        public FormSendeplan()
        {
            instance = this;
            InitializeComponent();

            lastDownload = new DateTime(1990, 1, 1, 1, 1, 1);
            lblSendeplanTitle.Text = "Empfange Update..";
            PlanScheduleUpdateTimer.Enabled = true; //Download new Schedule Data with a short delay
        }

        private void FormSendeplan_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (lastDownload != null && downloadedScheduleDataOnce) //don't download again if the first update / download (called in constructor)  hasn't finished
                {
                    // Only download current new schedule data (json) if last update was less then minDefferenceMinutes Minutes ago.
                    var minutes = (DateTime.Now - lastDownload).TotalMinutes;
                    if (minutes < minDefferenceMinutes)
                    {
                        UpdateLabels(); //Just update GUI based on old json
                        return;
                    }
                    else
                    {
                        // Download new json schedule!
                        lblSendeplanTitle.Text = "Empfange Update..";
                        PlanScheduleUpdateTimer.Enabled = true;
                    }
                }

            }
        }

        private void PlanScheduleUpdateTimer_Tick(object sender, EventArgs e)
        {
            PlanScheduleUpdateTimer.Enabled = false;
            UpdateScheduleData();
        }
        private void SwitchApi()
        {
          //  MessageBox.Show("Ein Fehler mit dem Sendeplan ist aufgetreten. Werde zurückfallen auf " + (useRBTVAPI ? "Google" : "RBTV") + " API.");
            useRBTVAPI = !useRBTVAPI; //use google as fallback for rbtv api and rbtvapi as fallback for google.
        }

        public void UpdateScheduleData()
        {
            //Todo: Create interface for downloading and handling schedule data, this will make this class way more readable...
            if (useRBTVAPI)
            {
                DownloadScheduleDataFromRBTVApi();
            }
            else
            {
                DownloadScheduleDataFromGoogle();
            }

        }
        private void clearSendeplan()
        {
            flowLayoutPanel1.Controls.Clear();
        }
        public void UpdateLabels(int maxCount = 13)
        {
            if (jsonSendeplan == null || jsonSendeplan.Length <= 0)
            {
                instance.SwitchApi();
                return;
            }

            try
            {
                JArray result;
                //Update headline based on rbtv or google json syntax
                if (useRBTVAPI)
                {
                    JObject resultroot = JObject.Parse(jsonSendeplan);
                    result = (JArray)resultroot["schedule"];
                    lblSendeplanTitle.Text = "Sendeplan vom " + DateTime.Parse((string)result[0]["timeStart"], CultureInfo.CreateSpecificCulture("en-us")).ToString("dd.MM.yyyy") + " (RBTV Sendeplan)\n\n";
                }
                else
                {
                    JObject resultroot = JObject.Parse(jsonSendeplan);
                    result = (JArray)resultroot["items"];
                    lblSendeplanTitle.Text = "Sendeplan vom " + DateTime.Parse((string)result[0]["start"]["dateTime"], CultureInfo.CreateSpecificCulture("en-us")).ToString("dd.MM.yyyy") + " (Google Kalender)\n\n";
                }

                // remove all existing labels, if there are too many labels in the flow control
                if (Math.Min(maxCount, result.Count) < flowLayoutPanel1.Controls.Count)
                {
                    clearSendeplan();
                }



                // if a valid json was recieved, but it doesn't contain any entires, switch api!
                if (result.Count < 1)
                {
                    SwitchApi();
                    return;
                }

                bool second = false; //  used for alternating colors
                int counter = 0;    // keeps track of the count of shown entries. if maxCount is smaller than the amounf of entries in the json response, this will be used to break the for)
                DateTime lastend = new DateTime(1990, 1, 1, 1, 1, 1); //will be used to detect fillers (if ((lastend-start).TotalMinutes) > 5 -> Filler Label hinzufügen!)

                foreach (JObject item in result)
                {
                    bool isNew = false;
                    DateTime start = DateTime.Now, end = DateTime.Now;
                    Label tmpLabel;

                    // Use existing labels if possible, add new ones otherwise.
                    if (counter < flowLayoutPanel1.Controls.Count)
                    {
                        tmpLabel = (Label)flowLayoutPanel1.Controls[counter];
                    }
                    else
                    {
                        tmpLabel = new Label();
                        tmpLabel.Font = new Font(tmpLabel.Font.FontFamily, 11);
                        tmpLabel.Width = 600;
                        flowLayoutPanel1.Controls.Add(tmpLabel);
                        isNew = true;
                    }

                    //Parse json and update label depending on api
                    if (useRBTVAPI)
                    {
                        // Parse json and update label based on rbtv API syntax
                        CreateLabelFromRBTVJSon(item, out start, out end, tmpLabel);
                    }
                    else
                    {
                        // Parse json and update label based on google calender syntax
                        CreateLabelFromGoogleJson(item, out start, out end, tmpLabel);
                    }

                    // Mark current time in DarkRed
                    if (tmpLabel.BackColor != Color.DarkRed && start < DateTime.Now && end > DateTime.Now)
                    {
                        tmpLabel.BackColor = Color.DarkRed;

                    }
                    //Otherwise use light or dark grey (alternating)
                    else if (isNew || tmpLabel.BackColor == Color.DarkRed)
                    {
                        if (second)
                        {
                            tmpLabel.BackColor = Color.FromArgb(255, 50, 50, 50);

                        }
                        else
                        {
                            tmpLabel.BackColor = Color.FromArgb(255, 100, 100, 100);
                        }
                    }

                    second = !second;
                    if (counter > maxCount)
                    {
                        break;
                    }
                    counter++;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                SwitchApi();
                Hide();
            }
        }
#endregion
#region RBTV Sendeplan API
        private void DownloadScheduleDataFromRBTVApi()
        {
            // RBTV Api uses X-WSSE Authentication
            // Should be executed asyncronously in the future, so it doesn't freeze the UI while Downloading...
            string key = "XXX";
            string secret = "XXX";
            string id = "00000000-0000-0000-0000-000000000000";
            string created = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ssK").Trim(); // "2016-02-05T12:15:02+01:00";
            string nonce = id + created + RandomString(10).Trim();
            string sha1 = string.Join("", SHA1CryptoServiceProvider.Create().ComputeHash(Encoding.UTF8.GetBytes(nonce + created + secret)).Select(x => x.ToString("x2")));
            string sha1base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(sha1));
            string url = "http://api.rocketmgmt.de/schedule";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Headers.Clear();
                req.Method = "GET";
                req.Accept = "application/json";
                req.Headers.Add("Authorization", "WSSE profile=\"UsernameToken\"");
                req.Headers.Add("X-WSSE", "UsernameToken Username=\"" + key + "\", PasswordDigest=\"" + sha1base64 + "\", Nonce=\"" + Convert.ToBase64String(Encoding.UTF8.GetBytes(nonce)) + "\", Created=\"" + created + "\"");
                using (WebResponse res = req.GetResponse())
                {
                    using (System.IO.StreamReader st = new System.IO.StreamReader(res.GetResponseStream()))
                    {
                        jsonSendeplan = st.ReadToEnd();
                        lastDownload = DateTime.Now;
                        downloadedScheduleDataOnce = true;
                        UpdateLabels();
                    }
                }
            }
            catch (Exception ex)
            {
                SwitchApi();
            }
        }
        private void CreateLabelFromRBTVJSon(JObject item, out DateTime start, out DateTime end, Label tmpLabel)
        {
            start = DateTime.Parse((string)item["timeStart"], CultureInfo.CreateSpecificCulture("en-us"));
            end = DateTime.Parse((string)item["timeEnd"], CultureInfo.CreateSpecificCulture("en-us"));

            int id = int.Parse((string)item["id"]);
            string show = (string)item["show"];
            string topic = (string)item["topic"];
            string game = (string)item["game"];
            string type = (string)item["type"];
            string summary = (string)item["title"] + (topic.Length > 0 ? ": " + topic : (game.Length > 0 ? ": " + game : ""));
            string displayText = start.ToString("HH:mm") + " Uhr - " + System.Web.HttpUtility.HtmlDecode(summary);

            //shownLabels[id].Text

            tmpLabel.ForeColor = Color.White;
            if (type.Equals("live"))
            {
                tmpLabel.ForeColor = Color.LightGreen;
                summary = "[Live] " + summary;
            }
            else if (type.Equals("premiere"))
            {
                tmpLabel.ForeColor = Color.LightSalmon;
                summary = "[Premiere] " + summary;
            }
            tmpLabel.Text = displayText;
        }
        private string RandomString(int count)
        {
            Random random = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; ++i)
            {
                sb.Append(random.Next(10));
            }
            return sb.ToString();
        }
#endregion


#region GOOGLE CALENDER API
        private static void DownloadScheduleDataFromGoogle()
        {
            string startTime = DateTime.Now.ToString("yyyy-MM-dd\\T") + "06:00:00+01:00";//yyyy-mm-dd\\/dd\\/yyyy h\\:mm tt"
            string endTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd") + "T05:59:59+01:00";
            using (WebClient client = new WebClient())
            {
                string baseString = "https://www.googleapis.com/calendar/v3/calendars/h6tfehdpu3jrbcrn9sdju9ohj8@group.calendar.google.com/events?fields=items/summary,items/start,items/end&singleEvents=True&orderBy=startTime&maxResults=20&key=AIzaSyB6jqCSNRJqoqxbXkImse04n-ukwZ5LRjg";
                string timeMinString = "&timeMin=" + System.Web.HttpUtility.UrlEncode(startTime, Encoding.UTF8);
                string timeMaxString = "&timeMax=" + System.Web.HttpUtility.UrlEncode(endTime, Encoding.UTF8);
                //Console.WriteLine(baseString + timeMinString + timeMaxString);
                client.Encoding = System.Text.Encoding.UTF8;
                client.DownloadStringCompleted += DownloadStringFromGoogleCompleted;
                client.DownloadStringAsync(new Uri(baseString + timeMinString + timeMaxString));

            }
        }

        public static void DownloadStringFromGoogleCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                
                instance.jsonSendeplan = e.Result;
                instance.UpdateLabels();
                instance.lastDownload = DateTime.Now;
                instance.downloadedScheduleDataOnce = true;
            }
            else
            {
                instance.SwitchApi();
            }
        }

        private static void CreateLabelFromGoogleJson(JObject item, out DateTime start, out DateTime end, Label tmpLabel)
        {
            start = DateTime.Parse((string)item["start"]["dateTime"], CultureInfo.CreateSpecificCulture("en-us"));
            end = DateTime.Parse((string)item["end"]["dateTime"], CultureInfo.CreateSpecificCulture("en-us"));
            string summary = (string)item["summary"];

            tmpLabel.ForeColor = Color.White;
            if (summary.Contains("[L]"))
            {
                tmpLabel.ForeColor = Color.LightGreen;
            }
            else if (summary.Contains("[N]"))
            {
                tmpLabel.ForeColor = Color.LightSalmon;
            }
            
            tmpLabel.Text = start.ToString("HH:mm") + " Uhr - " + System.Web.HttpUtility.HtmlDecode(summary);
        }
#endregion


    }


    public static class SHA1Util
    {
        /// <summary>
        /// Compute hash for string encoded as UTF8
        /// </summary>
        /// <param name="s">String to be hashed</param>
        /// <returns>40-character hex string</returns>
        public static string SHA1HashStringForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return HexStringFromBytes(hashBytes);
        }
        public static byte[] SHA1HashForUTF8String(string s)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(s);

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return hashBytes;
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}
