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
using System.Collections.ObjectModel;
using self_studyhub.Models;

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

            RecentList.ItemsSource = RecentManager.RecentFiles;
        
        }

        private void RecentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var file = RecentList.SelectedItem as Recentfile;

            if (file == null)
                return;
            MessageBox.Show(
       "Name: " + file.FileName +
       "\nPath: " + file.FilePath
   );
            string fullPath = file.FilePath;


            if (!File.Exists(fullPath))
            {
                MessageBox.Show("File not found");
                return;
            }

            var main = Window.GetWindow(this) as MainWindow;

            if (main != null)
            {
                main.MainContent.Content = new pdfviewerpage(fullPath);
            }

        }

        private void OpenPDFButton_Click(object sender, RoutedEventArgs e)
        {
     
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";

            if (openFileDialog.ShowDialog() == true)
            {
 
                var main = Window.GetWindow(this) as MainWindow;

                if (main == null)
                {
                    MessageBox.Show("MainWindow is NULL");
                    return;
                }

                string selectedPath = openFileDialog.FileName;

                RecentManager.AddRecent(selectedPath);

                pdfviewerpage viewer = new pdfviewerpage(selectedPath);

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

                // ဒီနေရာမှာ PDF viewer ကို open လုပ်နိုင်တယ်
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = SearchBox.Text?.ToLower() ?? "";

            if (string.IsNullOrWhiteSpace(keyword))
            {
                RecentList.ItemsSource = RecentManager.RecentFiles;
                return;
            }

            var result = RecentManager.RecentFiles
     .Where(x => x.FileName.ToLower().Contains(keyword))
     .ToList();

            RecentList.ItemsSource = result;
        }
    }

   
}
