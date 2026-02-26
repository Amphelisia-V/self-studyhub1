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
            OpenPDFButton.Click += OpenPDFButton_Click;
            ContinueButton.Click += ContinueButton_Click;
            ViewNotesButton.Click += ViewNotesButton_Click;
        }

        private void OpenPDFButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedPath = openFileDialog.FileName;

                // Get MainWindow
                MainWindow main = (MainWindow)Application.Current.MainWindow;
                main.MainContent.Content = new pdfviewerpage(selectedPath);
            }
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Continue last session clicked");
        }

        private void ViewNotesButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("View notes clicked");
        }
    }
}
