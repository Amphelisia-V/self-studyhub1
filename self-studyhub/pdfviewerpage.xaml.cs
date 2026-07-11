using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using Microsoft.Win32;
using Newtonsoft.Json;
using self_studyhub.Models;
using Syncfusion.Pdf;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace self_studyhub.Pages
{
    public partial class pdfviewerpage : UserControl
    {

        private string originalPath;
        private string editedPath;
        private readonly HttpClient client = new HttpClient();
        private int userId;
        private int pdfId;
        public pdfviewerpage(string pdfPath, int userId, int pdfId)
        {
            InitializeComponent();

            this.userId = userId;
            this.pdfId = pdfId;

            originalPath = pdfPath;

            if (!File.Exists(originalPath))
            {
                MessageBox.Show("File not found");
                return;
            }

            editedPath = Path.Combine(
                     Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                     $"temp_{Guid.NewGuid()}.pdf"
             );

            File.Copy(originalPath, editedPath, true);
            PdfViewer.CurrentPageChanged += PdfViewer_CurrentPageChanged;
            // Load PDF into Syncfusion Viewer
            Loaded += Pdfviewerpage_Loaded;

        }
        private void Pdfviewerpage_Loaded(object sender, RoutedEventArgs e)
        {
            PdfViewer.Load(editedPath);
        }
        private async void OpenPdf_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PDF Files (*.pdf)|*.pdf";

            if (dialog.ShowDialog() == true)
            {
                originalPath = dialog.FileName;

                editedPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
    "temp_edited.pdf"
);

                File.Copy(originalPath, editedPath, true);
                // Add to Recent
                await SavePDFHistory(
           Path.GetFileName(originalPath),
           originalPath
       );

                PdfViewer.Load(editedPath);
            }
        }
       
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(editedPath)) return;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PDF Files (*.pdf)|*.pdf";

            if (dialog.ShowDialog() == true)
            {
                File.Copy(editedPath, dialog.FileName, true);
                await SavePDFHistory(
    Path.GetFileName(dialog.FileName),
    dialog.FileName
);

                MessageBox.Show("PDF saved successfully");
            }
        }
        private async Task SavePDFHistory(string fileName, string filePath)
        {
            var pdf = new RecentPDF
            {
                UserId = userId,
                FileName =fileName,
                FilePath = filePath,
                IsSaved = true,
                OpenedDate = DateTime.Now
            };


            string json = JsonConvert.SerializeObject(pdf);


            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );


            var response = await client.PostAsync(
                "https://localhost:7118/api/RecentPDFs",
                content
            );


            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("PDF save failed");
            }
        }
        private async void ClosePdf_Click(object sender, RoutedEventArgs e)
        {
            await SaveCurrentPage();

            var main = Window.GetWindow(this) as MainWindow;

            if (main != null)
            {
                main.MainContent.Content = main.PDFPageInstance;
            }
        }
        private int currentPage = 1;


        private void PdfViewer_CurrentPageChanged(object sender, EventArgs e)
        {
            currentPage = PdfViewer.CurrentPage;

            Console.WriteLine("Current Page: " + currentPage);
        }
        private async Task SaveCurrentPage()
        {
            var pdf = new RecentPDF
            {
                PdfId = pdfId,
                UserId = userId,
                FilePath = originalPath,
                CurrentPage = currentPage
            };


            string json = JsonConvert.SerializeObject(pdf);


            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            await client.PutAsync(
        "https://localhost:7118/api/RecentPDFs/page",
        content);
        }
   
       
    }
}