using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;
namespace VK
{
    public partial class Form1 : MetroForm
    {
        public static string getapi; // Готовый запрос
        public static string paramid; // Часть запроса (Айди)
        public static string paramc; // Часть запроса (кол во постов)

        public Form1(string[] args)
        {
            InitializeComponent();

            //Аргументы из вне
            if (args.Length != 0)
            {
                paramid = args[0];
                paramc = args[1];
            }
            else
            {
                paramid = "-35193970";
                paramc = "100";
            }
            getapi = "https://api.vk.com/method/wall.get.xml?owner_id=" + paramid + "&count=" + paramc; //создание запроса
            if (paramid == "-35193970") { metroLabel3.Text = "Group: Perception of music"; }
            if (paramid == "-74779558") { metroLabel3.Text = "Group: Music for coding"; }
        }

        /// Главная загрузка
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadMusic(getapi);
            notifyIcon1.ContextMenuStrip = metroContextMenu1;
        }

        //Функциия загрузки музыки
        //Отправка запроса, создание плейлста
        //В GetMusic - передать запрос
        public static List<String> urlmusic = new List<String>();
        public static List<String> Namemusic = new List<String>();

        private WMPLib.IWMPPlaylist PlayList;
        private WMPLib.IWMPMedia Media;

        public void LoadMusic(string GetMusic)
        {
            try
            {
                listBox1.Items.Clear();
                Namemusic.Clear();
                urlmusic.Clear();
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
                    if (audioTag["url"] != null) { url = audioTag["url"].InnerText; url = url.Split('?')[0]; }
                    Media = axWindowsMediaPlayer1.newMedia(url);
                    PlayList.appendItem(Media);
                    listBox1.Items.Add(artist + " – " + title);
                    urlmusic.Add(url);
                    Namemusic.Add(artist + " – " + title);
                }
                axWindowsMediaPlayer1.currentPlaylist = PlayList;
                metroLabel2.Text = "Load " + listBox1.Items.Count.ToString() + " items.";
            }
            catch { MessageBox.Show("Ошибка загрузки (LoadMusic ERROR)"); }
        }

        /// Трей

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
            if (e.Button == MouseButtons.Left)
            {
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
            catch { MessageBox.Show("Ошибка загрузки (LoadCastomButton ERROR)"); }
        }

        // Пресеты групп
        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                switch (GroupComboBox1.SelectedIndex)
                {
                    case 0:
                        LoadMusic("https://api.vk.com/method/wall.get.xml?owner_id=-35193970&count=100");
                        metroLabel3.Text = "Group: Perception of music";
                        break;

                    case 1:
                        LoadMusic("https://api.vk.com/method/wall.get.xml?owner_id=-74779558&count=100");
                        metroLabel3.Text = "Group: Music for coding";
                        break;

                    case 2:
                        LoadMusic("https://api.vk.com/method/wall.get.xml?owner_id=-46730819&count=100");
                        metroLabel3.Text = "Group: Rock|Рок";
                        break;
                    case 3:
                        LoadMusic("https://api.vk.com/method/wall.get.xml?owner_id=-30989255&count=100");
                        metroLabel3.Text = "Group: British Music";
                        break;
                    default: break;
                }
            }
            catch { MessageBox.Show("Ошибка загрузки (GroupComboBox1 ERROR)"); }
        }

        ///
        /// Кнопка Minimize  Maximize
        ///
        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (this.Width == 664) { this.Width = 344; metroButton2.Text = "Maximize"; } else { this.Width = 664; metroButton2.Text = "Minimize"; }
        }

        ///Скачка файлов
        private void metroButton3_Click(object sender, EventArgs e)
        {
            try { backgroundWorkerDownLoad.RunWorkerAsync(); }
            catch { MessageBox.Show("Ошибка загрузки (DownLoad ERROR)"); }
            metroButton3.Enabled = false;
            LoadCastomButton1.Enabled = false;
            metroButton1.Enabled = false;
            metroButton4.Enabled = true;
            NameFileCheckBox.Visible = false;
            metroButton5.Enabled = false;
        }

        ///Кнопка стоп
        private void metroButton4_Click(object sender, EventArgs e)
        {
            try { backgroundWorkerDownLoad.CancelAsync(); }
            catch { MessageBox.Show("Ошибка загрузки (CancelAsync ERROR)"); }
        }

        ///Поток скачки
        private void backgroundWorkerDownLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            DateTime thisDay = DateTime.Today;
            string DownLoad = Directory.GetCurrentDirectory() + "\\Music\\" + thisDay.ToString("d") + " id " + paramid;
            Directory.CreateDirectory(DownLoad);
            DirectoryInfo dirInfo = new DirectoryInfo(DownLoad);

            foreach (FileInfo file in dirInfo.GetFiles())
            {
                file.Delete();
            }
            WebClient webClient = new WebClient();
            int fullload = 0;
            foreach (String uu in urlmusic)
            {
                if (backgroundWorkerDownLoad.CancellationPending) { break; }
                foreach (String ur in Namemusic)
                {
                    try
                    {
                        if (NameFileCheckBox.Checked == true) { webClient.DownloadFile(uu, DownLoad + "\\" + fullload + ". " + ur + ".mp3"); }
                        if (NameFileCheckBox.Checked == false) { webClient.DownloadFile(uu, DownLoad + "\\" + ur + ".mp3"); }
                    }
                    catch { fullload--; }

                    fullload++;
                    int owerload = urlmusic.Count - fullload;
                    if (owerload <= 0) { backgroundWorkerDownLoad.CancelAsync(); }
                    Action action = () =>
                    {
                        metroProgressBar1.Minimum = 0;
                        metroProgressBar1.Maximum = urlmusic.Count;
                        metroProgressBar1.Step = 1;
                        metroProgressBar1.PerformStep();
                        StatusLoad.Text = "Загружаю: " + ur;
                        StatusLoadCount.Text = "Осталось: " + owerload.ToString();
                    };
                    Invoke(action);

                    if (backgroundWorkerDownLoad.CancellationPending)
                    {
                        Action actionend = () =>
                        {
                            metroProgressBar1.Value = 0;
                            StatusLoad.Text = "";
                            StatusLoadCount.Text = "";
                        };
                        Invoke(actionend);
                        break;
                    }
                }
            }
        }

        ///Поток скачки конец
        private void backgroundWorkerDownLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            metroButton3.Enabled = true;
            LoadCastomButton1.Enabled = true;
            metroButton1.Enabled = true;
            metroButton4.Enabled = false;
            NameFileCheckBox.Visible = true;
            metroButton5.Enabled = true;
        }
        ///Кнопка рандом воспр
        private void metroCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.setMode("shuffle", metroCheckBox1.Checked);
        }
        ///ПКМ по трею
        private void fdgfdgToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        ///Тоже трей
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }
        ///Кнопка Старт
        private void metroButton5_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            metroButton5.Enabled = false;
        }
        ///Поток загрузки PlayNow
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DateTime thisDay = DateTime.Today;
                string DownLoad = Directory.GetCurrentDirectory() + "\\Music\\PlayNow\\";
                Directory.CreateDirectory(DownLoad);
                WebClient webClient = new WebClient();
                webClient.DownloadFile(axWindowsMediaPlayer1.Ctlcontrols.currentItem.sourceURL, DownLoad + axWindowsMediaPlayer1.Ctlcontrols.currentItem.name + ".mp3");
                Action action = () =>
                {
                    InfoLabel7.Text = "Загруженно: " + axWindowsMediaPlayer1.Ctlcontrols.currentItem.name;
                    metroButton5.Enabled = true;
                };
                Invoke(action);
            }
            catch { }
        }
    }
}