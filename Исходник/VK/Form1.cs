using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace VK
{
    public partial class Form1 : MetroForm
    {
        public static string getapi, paramid, paramc, namedir; // Готовый запрос
        public List<PresetGroupList> ListPreset = new List<PresetGroupList>();

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
                metroLabel3.Text = "Group: Perception of music";
            }
            getapi = "https://api.vk.com/method/wall.get.xml?owner_id=" + paramid + "&count=" + paramc; //создание запроса
            notifyIcon1.ContextMenuStrip = metroContextMenu1;
            LoadMusic(getapi);
            listBox1.Focus();
            namedir = "Perception of music";


            ////Первый запуск\Загрузка настр
            try ////загрузка настр 
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<PresetGroupList>));
                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PM Music\Preset.cfg";
                FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                ListPreset = (List<PresetGroupList>)ser.Deserialize(file);
                file.Close();
            }
            catch {
                ////Создание стандарт настр
                try
                {
                    PresetGroupList DefPreset0 = new PresetGroupList();
                    PresetGroupList DefPreset1 = new PresetGroupList();

                    DefPreset0.Name = "Perception of music";
                    DefPreset1.Name = "Music for coding";

                    DefPreset0.DoneAPI = "https://api.vk.com/method/wall.get.xml?owner_id=-35193970&count=100";
                    DefPreset1.DoneAPI = "https://api.vk.com/method/wall.get.xml?owner_id=-74779558&count=100";

                    ListPreset.Add(DefPreset0);
                    ListPreset.Add(DefPreset1);

                    XmlSerializer ser = new XmlSerializer(typeof(List<PresetGroupList>));
                    string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PM Music\Preset.cfg";
                    Directory.CreateDirectory(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PM Music\");
                    FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                    ser.Serialize(file, ListPreset);
                    file.Close();
                }
                catch { }
            }

            for (int i = 0; i < ListPreset.Count; i++)
            {
                GroupComboBox1.Items.Add(ListPreset[i].Name);
                DelComboBox.Items.Add(ListPreset[i].Name);
            }

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////// Кнопки
        //
        //
        //
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        private void metroButton1_Click(object sender, EventArgs e) ////Кнопка загрузить (Список)
        {
            try
            {
                LoadMusic(ListPreset[GroupComboBox1.SelectedIndex].DoneAPI);
                metroLabel3.Text = "Group: " + ListPreset[GroupComboBox1.SelectedIndex].Name;
                namedir = ListPreset[GroupComboBox1.SelectedIndex].Name;

            }
            catch { MessageBox.Show("Ошибка"); }
        }

        private void addPresetButton_Click(object sender, EventArgs e) ////Кнопка добавления присета
        {
            try
            {
                addPresetButton.Enabled = false;
                double num = 0.0;
                    if (PresetTextBox.Text != "" && GetApiTextBox.Text != "" && CountApiTextBox2.Text != "" && double.TryParse(CountApiTextBox2.Text, out num) && double.TryParse(GetApiTextBox.Text, out num))
                {
                    PresetGroupList TempPreset = new PresetGroupList();
                    TempPreset.Name = PresetTextBox.Text;
                    if (IfUserCheckBoxADD.Checked)
                    {
                        if (int.Parse(CountApiTextBox2.Text) < 100) { TempPreset.DoneAPI = "https://api.vk.com/method/wall.get.xml?owner_id=" + int.Parse(GetApiTextBox.Text.Trim().Replace(" ", string.Empty)) + "&count=" + int.Parse(CountApiTextBox2.Text.Trim().Replace(" ", string.Empty)); }
                        else { TempPreset.DoneAPI = "https://api.vk.com/method/wall.get.xml?owner_id=" + int.Parse(GetApiTextBox.Text.Trim().Replace(" ", string.Empty)) + "&count=100"; }
                    }
                    else
                    {
                        if (int.Parse(CountApiTextBox2.Text) < 100) { TempPreset.DoneAPI = "https://api.vk.com/method/wall.get.xml?owner_id=-" + int.Parse(GetApiTextBox.Text.Trim().Replace(" ", string.Empty)) + "&count=" + int.Parse(CountApiTextBox2.Text.Trim().Replace(" ", string.Empty)); }
                        else { TempPreset.DoneAPI = "https://api.vk.com/method/wall.get.xml?owner_id=-" + int.Parse(GetApiTextBox.Text.Trim().Replace(" ", string.Empty)) + "&count=100"; }
                    }

                    ListPreset.Add(TempPreset);
                    SaveFillPreset();
                    GoodLabel.Text = "Добавлено: " + TempPreset.Name;
                    addPresetButton.Enabled = true;


                }
                else MessageBox.Show("Введите правильные данные!"); addPresetButton.Enabled = true;
            }
            catch { MessageBox.Show("Ошибка"); }

        }



        private void DelButton_Click(object sender, EventArgs e) ////Кнопка Удаление присета
        {
            try
            {
                DelGoodLabel.Text = "Удалено: " + ListPreset[DelComboBox.SelectedIndex].Name;
                ListPreset.RemoveAt(DelComboBox.SelectedIndex);
                SaveFillPreset();
            }
            catch { MessageBox.Show("Ошибка"); }
        }

        private void CloseButton_Click(object sender, EventArgs e) ////Закрыть настр
        {
            ShowPreset(false);
        }



        private void EditGroupButton_Click(object sender, EventArgs e) ////Открыть настр
        {
            ShowPreset(true);
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e) //// 2 Клик по трею 
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }
        ///Кнопка Старт
        private void metroButton5_Click(object sender, EventArgs e) //// Кнопка скачать
        {
            backgroundWorker1.RunWorkerAsync();
            metroButton5.Enabled = false;
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
                //Много проверок. Работает и норм
                double num = 0.0;
                if (IDTextBox.Text != "" && CountTextBox.Text != "" && double.TryParse(CountTextBox.Text, out num) && double.TryParse(IDTextBox.Text, out num))
                {
                    int counif = int.Parse(CountTextBox.Text);
                    if (IfUser.Checked) {
                        if (counif < 100) { LoadMusic("https://api.vk.com/method/wall.get.xml?owner_id=" + IDTextBox.Text.Trim().Replace(" ", string.Empty) + "&count=" + CountTextBox.Text.Trim().Replace(" ", string.Empty)); }
                        else { LoadMusic("https://api.vk.com/method/wall.get.xml?owner_id=" + IDTextBox.Text.Trim().Replace(" ", string.Empty) + "&count=100"); }
                    }
                    else {
                        if (counif < 100) { LoadMusic("https://api.vk.com/method/wall.get.xml?owner_id=-" + IDTextBox.Text.Trim().Replace(" ", string.Empty) + "&count=" + CountTextBox.Text.Trim().Replace(" ", string.Empty)); }
                        else { LoadMusic("https://api.vk.com/method/wall.get.xml?owner_id=-" + IDTextBox.Text.Trim().Replace(" ", string.Empty) + "&count=100"); }
                    }

                }
                namedir = IDTextBox.Text;
                metroLabel3.Text = "";
                if (IDTextBox.Text == "35193970") { metroLabel3.Text = "Group: Perception of music"; }
                if (IDTextBox.Text == "74779558") { metroLabel3.Text = "Group: Music for coding"; }
            }
            catch { MessageBox.Show("Ошибка загрузки (LoadCastomButton ERROR)"); }
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
            backgroundWorkerDownLoad.RunWorkerAsync();
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






        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // Фоновые потоки
        //
        //
        //
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        ////Поток скачки всё
        private void backgroundWorkerDownLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string DownLoad = Directory.GetCurrentDirectory() + "\\Music\\" + DateTime.Today.ToString("d") + "{" + namedir.Trim() + "}";
                Directory.CreateDirectory(DownLoad);

                foreach (FileInfo file in new DirectoryInfo(DownLoad).GetFiles())
                {
                    file.Delete();
                }

                WebClient webClient = new WebClient();
                bool NameFile = false;
                if (NameFileCheckBox.Checked == true) NameFile = true;
                    for (int i = 0; i <= urlmusic.Count;) {
                    if (backgroundWorkerDownLoad.CancellationPending)
                    {
                        break;
                    }

                    if (NameFile == true) {
                        webClient.DownloadFile(urlmusic[i], DownLoad + "\\" + i + ". " + Namemusic[i] + ".mp3");
                    }
                    else {
                        webClient.DownloadFile(urlmusic[i], DownLoad + "\\" + Namemusic[i] + ".mp3");
                    }
                    Action action = () =>
                    {
                        metroProgressBar1.Minimum = 0;
                        metroProgressBar1.Maximum = urlmusic.Count;
                        metroProgressBar1.Step = 1;
                        metroProgressBar1.PerformStep();
                        StatusLoad.Text = "Загружаю: " + Namemusic[i];
                        StatusLoadCount.Text = "Осталось: " + (urlmusic.Count - i).ToString();
                    };
                    Invoke(action);
                    i++;
                }
            }
            catch { MessageBox.Show("Ошибка загрузки"); }
        }

        ///Поток скачки конец
        private void backgroundWorkerDownLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            metroProgressBar1.Value = 0;
            StatusLoad.Text = "";
            StatusLoadCount.Text = "";
            metroButton3.Enabled = true;
            LoadCastomButton1.Enabled = true;
            metroButton1.Enabled = true;
            metroButton4.Enabled = false;
            NameFileCheckBox.Visible = true;
            metroButton5.Enabled = true;
        }




        ///Поток загрузки PlayNow
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
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
            catch { MessageBox.Show("Ошибка загрузки (backgroundrDownLoadPlayNow ERROR)"); }
        }





        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // Прочие
        //
        //
        //
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //// Класс листа персета
        public class PresetGroupList
        {
            public string Name { get; set; }
            public string DoneAPI { get; set; }
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




        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // Функции
        //
        //
        //
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void ShowPreset(bool show) //Показ настр
        {
            PresetLabel7.Visible = show;
            metroLabel7.Visible = show;
            metroLabel8.Visible = show;
            GetApiTextBox.Visible = show;
            CountApiTextBox2.Visible = show;
            PresetTextBox.Visible = show;
            IfUserCheckBoxADD.Visible = show;
            addPresetButton.Visible = show;
            DelComboBox.Visible = show;
            DelButton.Visible = show;
            CloseButton.Visible = show;

            if (show == true) { pictureBox1.Visible = false; EditGroupButton.Enabled = false; }
            else { pictureBox1.Visible = true; EditGroupButton.Enabled = true; GoodLabel.Visible = false; DelGoodLabel.Visible = false; }
        }

        public void SaveFillPreset() //Обновление списков, сохр настр
        {
            try
            {
                DelComboBox.Items.Clear();
                GroupComboBox1.Items.Clear();

                for (int i = 0; i < ListPreset.Count; i++)
                {
                    GroupComboBox1.Items.Add(ListPreset[i].Name);
                    DelComboBox.Items.Add(ListPreset[i].Name);
                }
                //Сохранение настр в мои документы в папку PM Music
                XmlSerializer ser = new XmlSerializer(typeof(List<PresetGroupList>));
                string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PM Music\Preset.cfg";
                Directory.CreateDirectory(System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PM Music\");
                FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                ser.Serialize(file, ListPreset);
                file.Close();
            }
            catch { MessageBox.Show("Ошибка"); }
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
                int id = 0;
                foreach (XmlNode audioTag in audioTags)
                {
                    string artist = "";
                    string title = "";
                    string url = "";
                    if (audioTag["artist"] != null) artist = audioTag["artist"].InnerText;
                    if (audioTag["title"] != null) title = audioTag["title"].InnerText;
                    if (audioTag["url"] != null) { url = audioTag["url"].InnerText; url = url.Split('?')[0]; }
                    if(url != "") { 
                    Media = axWindowsMediaPlayer1.newMedia(url);
                    if (Media.isReadOnlyItem("title") == false) Media.setItemInfo("title", title);
                    if (Media.isReadOnlyItem("artist") == false) Media.setItemInfo("artist", artist);
                    if (Media.isReadOnlyItem("Id") == false) Media.setItemInfo("Id", id.ToString());
                    PlayList.appendItem(Media);
                    listBox1.Items.Add(artist + " – " + title);
                    urlmusic.Add(url);
                    Namemusic.Add(artist + " – " + title);
                    id++;
                    }
            }
                axWindowsMediaPlayer1.currentPlaylist = PlayList;
                metroLabel2.Text = "Load " + listBox1.Items.Count.ToString() + " items.";
          }

           catch { MessageBox.Show("Ошибка загрузки (LoadMusic ERROR)"); }
        }
       
        //АвтоВыбор песни, которая играет
        private void axWindowsMediaPlayer1_CurrentItemChange(object sender, AxWMPLib._WMPOCXEvents_CurrentItemChangeEvent e)
        {
            if (!listBox1.ContainsFocus) {
                listBox1.SetSelected(int.Parse(axWindowsMediaPlayer1.currentMedia.getItemInfo("Id")), true);
            }
        }
    }
}