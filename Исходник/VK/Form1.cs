using System;
using System.Windows.Forms;
using System.Xml;
using MetroFramework.Forms;

namespace VK
{
    public partial class Form1 : MetroForm
    {
        public static string getapi; // Готовый запрос
        public static string paramid; // Часть запроса (Айди)
        public static string paramc; // Часть запроса (кол во постов)

        public static string PMgetMusic = "https://api.vk.com/method/wall.get.xml?owner_id=-35193970&count="; //Пресет
        public static string MFCgetMusic = "https://api.vk.com/method/wall.get.xml?owner_id=-74779558&count="; // Тоже

        public string usedurl; //Хуй знает что, забыл 
        public Form1(string[] args)
        {
            InitializeComponent();

            //Аргументы из вне
            if (args.Length != 0)
            {
                paramid = args[0];
                paramc = args[1];
            }
            else {
                paramid = "-35193970";
                paramc = "100";
            }
            getapi = "https://api.vk.com/method/wall.get.xml?owner_id=" + paramid + "&count=" + paramc; //создание запроса
            if (paramid == "-35193970") { metroLabel3.Text = "Group: Perception of music";} 
            if (paramid == "-74779558") { metroLabel3.Text = "Group: Music for coding";}
        }


        //Функциия загрузки музыки
        //Отправка запроса, создание плейлста
        //В GetMusic - передать запрос
        public void LoadMusic(string GetMusic) {
           try
            {
                listBox1.Items.Clear();
                WMPLib.IWMPPlaylist PlayList;
                WMPLib.IWMPMedia Media;

                var doc = new XmlDocument();
                doc.Load(new XmlTextReader(GetMusic));
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
                    listBox1.Items.Add(artist + " – " + title);

                }
                axWindowsMediaPlayer1.currentPlaylist = PlayList;
                metroLabel2.Text = "Load " + listBox1.Items.Count.ToString() + " items.";
            }
           catch{MessageBox.Show("Ошибка загрузки(LoadMusic ERROR)");}
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            LoadMusic(getapi);
        }

        //иконка в треии
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

        //Выбор песни кликом в плейлисте
        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                axWindowsMediaPlayer1.Ctlcontrols.currentItem = axWindowsMediaPlayer1.currentPlaylist.get_Item(listBox1.SelectedIndex);
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }
        //Загрузка группы по настройкам (Кастомная)
        private void LoadCastomButton1_Click(object sender, EventArgs e)
        {
            try
            {
                //Хуева туча проверок. Работает и норм
                double num = 0.0;
                if (IDTextBox.Text != "" && CountTextBox.Text != "" && double.TryParse(CountTextBox.Text, out num) && double.TryParse(IDTextBox.Text, out num))
                {
                    int counif = int.Parse(CountTextBox.Text);
                    if (counif < 100) { LoadMusic("https://api.vk.com/method/wall.get.xml?owner_id=-" + IDTextBox.Text + "&count=" + CountTextBox.Text); }
                    else { LoadMusic("https://api.vk.com/method/wall.get.xml?owner_id=-" + IDTextBox.Text + "&count=100"); }
                }
                metroLabel3.Text = "";
                if (IDTextBox.Text == "35193970") { metroLabel3.Text = "Group: Perception of music"; }
                if (IDTextBox.Text == "74779558") { metroLabel3.Text = "Group: Music for coding"; }
            }
            catch {MessageBox.Show("Ошибка загрузки (LoadCastomButton ERROR)");}
        }
        // Пресеты групп
        private void metroButton1_Click(object sender, EventArgs e)
        {
            try { 
            switch(GroupComboBox1.SelectedIndex) {
                case 0:
                    LoadMusic(PMgetMusic + "100");
                    metroLabel3.Text = "Group: Perception of music";
                    break;
                case 1:
                    LoadMusic(MFCgetMusic + "100");
                    metroLabel3.Text = "Group: Music for coding";
                    break;
                default: break;
                }
            }
            catch { MessageBox.Show("Ошибка загрузки (GroupComboBox1 ERROR)"); }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (this.Width == 664) { this.Width = 344; metroButton2.Text = "Maximize"; } else { this.Width = 664; metroButton2.Text = "Minimize"; }
        }
    }
}
