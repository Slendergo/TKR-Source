using System;
using System.IO;
using System.Windows.Forms;

namespace AutomatedDatabaseBackup
{
    public partial class Form1 : Form
    {
        CountDownTimer timer = new CountDownTimer();

        public Form1()
        {
            InitializeComponent();

            //update label text
            timer.TimeChanged += () => label1.Text = timer.TimeLeftMsStr;

            // show messageBox on timer = 00:00.000
            timer.CountDownFinished += () =>
            {
                File.Copy(FileInfo.FullName, $"{DirFilePath}\\{Path.GetFileNameWithoutExtension(FileInfo.FullName)} {DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss")}{Path.GetExtension(FileInfo.FullName)}");
                timer.Restart();
            };

            //timer step. By default is 1 second
            timer.StepMs = 16;
        }

        public FileInfo FileInfo { get; set; }
        public string DirFilePath { get; set; }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                //openFileDialog.Filter = "redis database (*.rdb)|*.rdb";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    FileInfo = new FileInfo(openFileDialog.FileName);
            }
        }

        public bool Started { get; set; }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    DirFilePath = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (FileInfo == null)
            {
                MessageBox.Show("Select a file path");
                return;
            }

            if (DirFilePath == null)
            {
                MessageBox.Show("Select a directory path");
                return;
            }

            Started = true;

            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = false;
            button4.Enabled = false;

            timer.SetTime(5, 0);
            timer.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Started = false;

            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = true;

            timer.SetTime(5, 0);
            timer.Stop();
        }
    }
}
