using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32; // For OpenFileDialog
using System.IO;
using System.Diagnostics;
using self_studyhub.Pages;

namespace self_studyhub.Pages
{
    /// <summary>
    /// Interaction logic for pdf.xaml
    /// </summary>
    public partial class PDFPage : UserControl
    {
        public PDFPage()
        {
            InitializeComponent();
            // Hook up buttons
           
        }

        private void RecentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecentList.SelectedItem != null)
            {
                ListBoxItem item = (ListBoxItem)RecentList.SelectedItem;

                string filename = item.Content.ToString();

                string fullPath = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    filename);

                MainWindow main = Application.Current.MainWindow as MainWindow;

                if (main != null)
                {
                    main.MainContent.Content = new pdfviewerpage(fullPath);
                }
            }
        }

        private void OpenPDFButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Step 1");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";

            if (openFileDialog.ShowDialog() == true)
            {
                MessageBox.Show("Step 2");

                var main = Application.Current.MainWindow as MainWindow;

                if (main == null)
                {
                    MessageBox.Show("MainWindow is NULL");
                    return;
                }

                MessageBox.Show("Step 3");

                string selectedPath = openFileDialog.FileName;

                pdfviewerpage viewer = new pdfviewerpage(selectedPath);

                MessageBox.Show("Step 4");

                main.MainContent.Content = viewer;
            }
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            string file = "lastsession.txt";

            if (File.Exists(file))
            {
                string[] data = File.ReadAllLines(file);

                string pdfPath = data[0];
                int page = int.Parse(data[1]);

                MessageBox.Show("Opening Last Session\nPDF: " + pdfPath + "\nPage: " + page);

                //ဒီနေရာမှာ PDF viewer ကို open လုပ်နိုင်တယ်
            }
            else
            {
                MessageBox.Show("No previous session found.");
            }
        }

        private void ViewNotesButton_Click(object sender, RoutedEventArgs e)
        {
            string noteFile = "notes.txt";

            if (File.Exists(noteFile))
            {
                string notes = File.ReadAllText(noteFile);

                MessageBox.Show(notes, "My Notes");
            }
            else
            {
                MessageBox.Show("No notes found.");
            }
        }
    }

   
}
