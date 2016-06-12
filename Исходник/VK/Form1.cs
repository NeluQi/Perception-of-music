using System;
using System.Windows.Forms;
using System.Xml;
namespace VK
{
    public partial class Form1 : Form
    {
        public string usedurl;
        public Form1()
        {
            InitializeComponent();

            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            var doc = new XmlDocument();
            doc.Load(new XmlTextReader("https://api.vk.com/method/wall.get.xml?owner_id=-35193970&count=100"));

            var audioTags = doc.SelectNodes("//audio");
            PlayList = axWindowsMediaPlayer1.playlistCollection.newPlaylist("vkPlayList");
    
                MessageBox.Show("Загрузка успешна");
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
            MessageBox.Show("Специально для группы vk.com/pmpage \n Автор - vk.com/id208497682 \n Обвновления искать на github.com/egor2998067 \n Кода Вам без багов :)");
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
