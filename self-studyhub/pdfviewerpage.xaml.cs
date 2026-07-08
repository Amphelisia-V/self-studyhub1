using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;


namespace self_studyhub.Pages
{
    public partial class pdfviewerpage : UserControl
    {
        private string originalPath;
        private string editedPath;

        public pdfviewerpage(string pdfPath)
        {
            InitializeComponent();
   
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

            // Load PDF into Syncfusion Viewer
            Loaded += Pdfviewerpage_Loaded;
        }
        private void Pdfviewerpage_Loaded(object sender, RoutedEventArgs e)
        {
            PdfViewer.Load(editedPath);
        }
        private  void OpenPdf_Click(object sender, RoutedEventArgs e)
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
                Models.RecentManager.AddRecent(originalPath);

                PdfViewer.Load(editedPath);
            }
        }
       
        private void Highlight_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(editedPath))
                return;


            string tempPath = editedPath.Replace(".pdf", "_temp.pdf");


            using (PdfDocument pdfDoc =
                new PdfDocument(
                    new PdfReader(editedPath),
                    new PdfWriter(tempPath)))
            {

                var page = pdfDoc.GetPage(1);


                var rect =
                    new iText.Kernel.Geom.Rectangle(100, 600, 200, 20);


                var highlight =
                    PdfTextMarkupAnnotation.CreateHighLight(
                        rect,
                        new float[]
                        {
                            100,600,
                            300,600
                        });


                page.AddAnnotation(highlight);
            }


            File.Delete(editedPath);
            File.Move(tempPath, editedPath);


            PdfViewer.Load(editedPath);
        }

        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(editedPath))
                return;


            string tempPath = editedPath.Replace(".pdf", "_temp.pdf");


            using (PdfDocument pdfDoc =
                new PdfDocument(
                    new PdfReader(editedPath),
                    new PdfWriter(tempPath)))
            {

                var page = pdfDoc.GetPage(1);


                var rect =
                    new iText.Kernel.Geom.Rectangle(100, 550, 20, 20);


                var note = new PdfTextAnnotation(rect);

                note.SetContents("This is my note");


                page.AddAnnotation(note);
            }


            File.Delete(editedPath);
            File.Move(tempPath, editedPath);


            PdfViewer.Load(editedPath);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(editedPath)) return;

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PDF Files (*.pdf)|*.pdf";

            if (dialog.ShowDialog() == true)
            {
                File.Copy(editedPath, dialog.FileName, true);
            }
        }
    }
}