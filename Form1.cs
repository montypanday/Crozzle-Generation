using System;
using System.IO;
using System.Windows.Forms;
using Crozzle.ServiceReference1;

namespace Crozzle
{
    public partial class Form1 : Form
    {
        private CrozzleGrid crozzle;

        private System.Timers.Timer MyTimer;
        public Form1()
        {
            InitializeComponent();
            addressesCB.Items.Add("http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest1.czl");
            addressesCB.Items.Add("http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest2.czl");
            addressesCB.Items.Add("http://www.it.deakin.edu.au/SIT323/Task2/MarkingTest3.czl");
        }
        /// <summary>
        /// Stops the timer when time has elasped
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyTimer_Elasped(object sender, System.Timers.ElapsedEventArgs e)
        {
            MyTimer.Stop();
        }
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }
        private void addCrozzleBTN_Click(object sender, EventArgs e)
        {
            addressesCB.Items.Add(newAddressTB.Text);
        }
        /// <summary>
        /// This method builts a new crozzle after reading the files from the given destination.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getCrozzleBTN_Click(object sender, EventArgs e)
        {
            crozzle = new CrozzleGrid(addressesCB.Text);
            try
            {
                crozzle.ReadCrozzle();
                MyTimer = new System.Timers.Timer(crozzle.Config.RUNTIME_LIMIT * 1000);
                MyTimer.Elapsed += MyTimer_Elasped;
                MyTimer.Start();
                while (MyTimer.Enabled)
                {
                    crozzle.Fill();
                }

                string endpoint = "BasicHttpBinding_IWordGroupService";
                WordGroupServiceClient ws = new WordGroupServiceClient(endpoint);
                int NumberOfGroups = ws.Count(crozzle.GetGrid());
                if (NumberOfGroups >= crozzle.Config.MINIMUM_NUMBER_OF_GROUPS && NumberOfGroups <= crozzle.Config.MAXIMUM_NUMBER_OF_GROUPS)
                {
                    string result = crozzle.GetHtmlTable();
                    int score = crozzle.CalculateScore();
                    result += "<br/><div><p><b> Score = " + score + "</b></p></div>";
                    webBrowser1.Navigate("about:blank");
                    HtmlDocument doc = webBrowser1.Document;
                    doc.Write(String.Empty);
                    webBrowser1.DocumentText = result;
                }
                else
                {
                    MessageBox.Show("Crozzle cannot be formed because valid number of groups were not formed");
                    string result = crozzle.GetHtmlTable();
                    result += "<br/><div><p><b> Invalid Crozzle is Generated !</b></p></div>";
                    webBrowser1.Navigate("about:blank");
                    HtmlDocument doc = webBrowser1.Document;
                    doc.Write(String.Empty);
                    webBrowser1.DocumentText = result;
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Network Failure: Cannot Read Crozzle Configurations from the given address!");
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "czl";
            saveFileDialog1.Filter = "Crozzle Files (*.czl)|*.czl";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                // Get file name.
                string name = saveFileDialog1.FileName;
                // Write to the file name selected.
                // ... You can write the text from a TextBox instead of a string literal.
                string FileContents = crozzle.GetFileContents();
                File.WriteAllText(name, FileContents);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please select and build a Crozzle before trying to save it");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.Filter = "Crozzle Files (*.czl)|*.czl";
            if (openFileDialog1.ShowDialog() != DialogResult.Cancel)
            {
                crozzle = new CrozzleGrid();
                crozzle.Path = openFileDialog1.FileName;
                crozzle.ReadFile();
                crozzle.Load();
                crozzle.ReadWordsFromLocalFile();
                string result = crozzle.GetHtmlTable();
                int score = crozzle.CalculateScore();
                result += "<br/><div><p><b> Score = " + score + "</b></p></div>";
                webBrowser1.Navigate("about:blank");
                HtmlDocument doc = webBrowser1.Document;
                doc.Write(String.Empty);
                webBrowser1.DocumentText = result;
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
