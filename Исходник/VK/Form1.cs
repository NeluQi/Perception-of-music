using System;
using System.Windows.Forms;
using System.Xml;
namespace VK
{
    public partial class Form1 : Form
    {
        public static string getapi;
        public string usedurl;
        public Form1(string[] args)
        {
            InitializeComponent();
            if (args.Length != 0)
            {
                string paramid = args[0];
                string paramc = args[1];
                getapi = "https://api.vk.com/method/wall.get.xml?owner_id=" + paramid + "&count=" + paramc;
            }
            else { getapi = "https://api.vk.com/method/wall.get.xml?owner_id=-35193970&count=100"; }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {
                var doc = new XmlDocument();
                doc.Load(new XmlTextReader(getapi));

                var audioTags = doc.SelectNodes("//audio");
                PlayList = axWindowsMediaPlayer1.playlistCollection.newPlaylist("vkPlayList");


                foreach (XmlNode audioTag in audioTags)
                {
                    string artist = "";
                    string title = "";
                    string url = "";
                    if (audioTag["artist"] != null) artist = audioTag["artist"].InnerText;
                    if (audioTag["title"] != null) title = audioTag["title"].InnerText;
                    if (audioTag["url"] != null) url = audioTag["url"].InnerText;
                    Media = axWindowsMediaPlayer1.newMedia(url);
                    PlayList.appendItem(Media);
                    listBox1.Items.Add(title + " - " + artist);

                }
                axWindowsMediaPlayer1.currentPlaylist = PlayList;
            }

            catch {
                MessageBox.Show("Ошибка загрузки");
            }
        }
        WMPLib.IWMPPlaylist PlayList;
        WMPLib.IWMPMedia Media;
           


        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Автор - vk.com/id208497682 \n Обвновления искать на github.com/egor2998067");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Visible = true;
            button3.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
            listBox1.Visible = false;
        }
    }
    
}
