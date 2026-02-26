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
using Microsoft.Win32;
using System.IO;
using Syncfusion.Windows.PdfViewer;

namespace self_studyhub.Pages
{
    /// <summary>
    /// Interaction logic for pdfviewerpage.xaml
    /// </summary>
    public partial class pdfviewerpage : UserControl
    {
        string currentFilePath = "";
        public pdfviewerpage(string filePath)
        {
            InitializeComponent();
            currentFilePath = filePath;
            if (!string.IsNullOrEmpty(filePath))
            {
                pdfViewer.Load(filePath);
            }
        }

        private void OpenPdf_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PDF Files (*.pdf)|*.pdf";

            if (dialog.ShowDialog() == true)
            {
                currentFilePath = dialog.FileName;
                pdfViewer.Load(currentFilePath);
            }
        }

        private void Highlight_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Highlight feature depends on Syncfusion full annotation package.");
        }

        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sticky note feature requires annotation-enabled version.");
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                pdfViewer.Save(currentFilePath);
                MessageBox.Show("Saved Successfully!");
            }
        }
    }
}
