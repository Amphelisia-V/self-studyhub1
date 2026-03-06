using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Geom;
using iText.Kernel.Colors;

namespace self_studyhub.Pages
{
    public partial class pdfviewerpage : UserControl
    {
        private string originalPath; // original PDF
        private string editedPath;   // copy for editing

        public pdfviewerpage(string pdfPath)
        {
            InitializeComponent();
            originalPath = pdfPath;

            editedPath = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(originalPath),
                System.IO.Path.GetFileNameWithoutExtension(originalPath) + "_edited.pdf"
            );

            File.Copy(originalPath, editedPath, true);

            LoadPdf();
        }
        private async void LoadPdf()
        {
            await PdfViewer.EnsureCoreWebView2Async();
            PdfViewer.Source = new Uri(editedPath);
        }

        // ================= OPEN PDF =================
        private async void OpenPdf_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PDF Files (*.pdf)|*.pdf";

            if (dialog.ShowDialog() == true)
            {
                originalPath = dialog.FileName;

                // copy original to edited
                editedPath = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(originalPath),
                    System.IO.Path.GetFileNameWithoutExtension(originalPath) + "_edited.pdf"
                );

                File.Copy(originalPath, editedPath, true);

                // initialize WebView2 and load PDF
                await PdfViewer.EnsureCoreWebView2Async();
                PdfViewer.Source = new Uri(editedPath);

                MessageBox.Show("PDF copied and loaded!");
            }
        }

        // ================= HIGHLIGHT =================
        private void Highlight_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(editedPath))
            {
                MessageBox.Show("Open a PDF first.");
                return;
            }

            string tempPath = editedPath.Replace(".pdf", "_temp.pdf");

            using (PdfDocument pdfDoc = new PdfDocument(
                new PdfReader(editedPath),
                new PdfWriter(tempPath)))
            {
                PdfPage page = pdfDoc.GetPage(1); // demo: always first page

                // Rectangle for highlight (demo coordinates)
                iText.Kernel.Geom.Rectangle rect = new iText.Kernel.Geom.Rectangle(100, 600, 200, 20);

                // create highlight annotation
                var highlight = PdfTextMarkupAnnotation.CreateHighLight(rect, new float[] { 100, 600, 300, 600 });
                highlight.SetColor(ColorConstants.YELLOW);

                page.AddAnnotation(highlight);
            }

            // safely replace edited file
            File.Delete(editedPath);
            File.Move(tempPath, editedPath);

            PdfViewer.Source = new Uri(editedPath);

            MessageBox.Show("Highlight added!");
        }

        // ================= ADD NOTE =================
        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(editedPath))
            {
                MessageBox.Show("Open a PDF first.");
                return;
            }

            string tempPath = editedPath.Replace(".pdf", "_temp.pdf");

            using (PdfDocument pdfDoc = new PdfDocument(
                new PdfReader(editedPath),
                new PdfWriter(tempPath)))
            {
                PdfPage page = pdfDoc.GetPage(1); // demo: first page

                // Rectangle for sticky note
                iText.Kernel.Geom.Rectangle rect = new iText.Kernel.Geom.Rectangle(100, 550, 20, 20);

                // Explicit cast
                PdfTextAnnotation note = (PdfTextAnnotation)new PdfTextAnnotation(rect);
                note.SetContents("This is my note");
                note.Put(iText.Kernel.Pdf.PdfName.Open, iText.Kernel.Pdf.PdfBoolean.TRUE);

                page.AddAnnotation(note);
            }

            File.Delete(editedPath);
            File.Move(tempPath, editedPath);

            PdfViewer.Source = new Uri(editedPath);
            MessageBox.Show("Note added!");
        }

        // ================= SAVE AS =================
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(editedPath))
            {
                MessageBox.Show("Nothing to save.");
                return;
            }

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "PDF Files (*.pdf)|*.pdf";

            if (dialog.ShowDialog() == true)
            {
                File.Copy(editedPath, dialog.FileName, true);
                MessageBox.Show("Saved Successfully!");
            }
        }
    }
}