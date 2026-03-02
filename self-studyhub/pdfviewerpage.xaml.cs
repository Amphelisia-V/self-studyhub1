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
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Annot;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using System.IO;


namespace self_studyhub.Pages
{
   
    /// <summary>
    /// Interaction logic for pdfviewerpage.xaml
    /// </summary>
    public partial class pdfviewerpage : UserControl
    {
        string currentFilePath = "";

        // ✅ Fake Database
        List<PdfNote> fakeDatabase = new List<PdfNote>();

        public pdfviewerpage(string filePath)
        {
            InitializeComponent();
            currentFilePath = filePath;
        }

        private async void OpenPdf_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "PDF Files (*.pdf)|*.pdf";

            if (dialog.ShowDialog() == true)
            {
                currentFilePath = dialog.FileName;

                await PdfViewer.EnsureCoreWebView2Async();
                PdfViewer.Source = new Uri(currentFilePath);
            }
        }

        private async void Highlight_Click(object sender, RoutedEventArgs e)
        {
            string selectedText = await PdfViewer.ExecuteScriptAsync("window.getSelection().toString();");
            selectedText = selectedText.Replace("\"", "");

            if (!string.IsNullOrWhiteSpace(selectedText))
            {
                // Add to sidebar
                BookmarkList.Items.Add("🟡 " + selectedText);
            }
            else
            {
                MessageBox.Show("Please select text first.");
            }
        }

        private void AddNote_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                MessageBox.Show("Please open a PDF first.");
                return;
            }

            // WPF native input
            Window inputWindow = new Window
            {
                Title = "Add Note",
                Width = 300,
                Height = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            StackPanel stack = new StackPanel();
            TextBox textBox = new TextBox { Margin = new Thickness(10) };
            Button okButton = new Button { Content = "OK", Width = 60, Margin = new Thickness(10) };
            stack.Children.Add(textBox);
            stack.Children.Add(okButton);
            inputWindow.Content = stack;

            okButton.Click += (s, ev) => inputWindow.DialogResult = true;

            if (inputWindow.ShowDialog() == true)
            {
                string noteText = textBox.Text;
                if (!string.IsNullOrWhiteSpace(noteText))
                {
                    BookmarkList.Items.Add("📝 " + noteText);
                    fakeDatabase.Add(new PdfNote
                    {
                        FilePath = currentFilePath,
                        SelectedText = noteText,
                        CreatedDate = DateTime.Now
                    });
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (BookmarkList.Items.Count == 0)
            {
                MessageBox.Show("No notes to save.");
                return;
            }

            foreach (var item in BookmarkList.Items)
            {
                PdfNote note = new PdfNote()
                {
                    FilePath = currentFilePath,
                    SelectedText = item.ToString(),
                    CreatedDate = DateTime.Now
                };

                fakeDatabase.Add(note);
            }

            MessageBox.Show("Saved to Fake Database!");
        }
    }
}
