using Microsoft.Win32; // For OpenFileDialog
using self_studyhub.Models;
using self_studyhub.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;

namespace self_studyhub.Pages
{
    /// <summary>
    /// Interaction logic for pdf.xaml
    /// </summary>
    public partial class PDFPage : UserControl
    {
        public ObservableCollection<RecentPDF> RecentPDFs { get; set; }

        private readonly HttpClient client = new HttpClient();

        private int userId;
        public PDFPage(int userId)
        {
            InitializeComponent();

            this.userId = userId;

            RecentPDFs = new ObservableCollection<RecentPDF>();

            RecentList.ItemsSource = RecentPDFs;

            this.Loaded += PDFPage_Loaded;

        }
        private async void PDFPage_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadRecentPDFs();
        }
        private async Task LoadRecentPDFs()
        {
            try
            {
                var response = await client.GetAsync(
                    $"https://localhost:7118/api/RecentPDFs/{userId}"
                );


                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();


                    var pdfs = JsonConvert.DeserializeObject<List<RecentPDF>>(json);


                    RecentPDFs.Clear();


                    if (pdfs != null)
                    {
                        foreach (var pdf in pdfs)
                        {
                            RecentPDFs.Add(pdf);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task<RecentPDF> SaveRecentPDF(string fileName, string filePath)
        {
            try
            {
                var pdf = new RecentPDF
                {
                    UserId = userId,
                    FileName = fileName,
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


                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    var savedPdf = JsonConvert.DeserializeObject<RecentPDF>(result);

                    await LoadRecentPDFs();

                    return savedPdf;
                }
                else
                {
                    MessageBox.Show("PDF save failed");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private void RecentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var file = RecentList.SelectedItem as RecentPDF;

            if (file == null)
                return;

          
            string fullPath = file.FilePath;


            if (!File.Exists(fullPath))
            {
                MessageBox.Show("File not found");
                return;
            }

            var main = Window.GetWindow(this) as MainWindow;

            if (main != null)
            {
                main.MainContent.Content = new pdfviewerpage(fullPath, userId, file.PdfId);
            }

        }

        private async void OpenPDFButton_Click(object sender, RoutedEventArgs e)
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


                var savedPdf = await SaveRecentPDF(
       System.IO.Path.GetFileName(selectedPath),
       selectedPath
   );


                if (savedPdf != null)
                {
                    pdfviewerpage viewer =
                        new pdfviewerpage(
                            selectedPath,
                            userId,
                            savedPdf.PdfId
                        );

                    main.MainContent.Content = viewer;
                }
            }
        }
     
      
        private async void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = await client.GetAsync(
                    $"https://localhost:7118/api/RecentPDFs/last/{userId}"
                );


                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();


                    var pdf = JsonConvert.DeserializeObject<RecentPDF>(json);


                    if (pdf != null)
                    {
                        if (!File.Exists(pdf.FilePath))
                        {
                            MessageBox.Show("PDF file not found");
                            return;
                        }


                        var main = Window.GetWindow(this) as MainWindow;


                        if (main != null)
                        {
                            main.MainContent.Content =
                                new pdfviewerpage(pdf.FilePath, userId, pdf.PdfId);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No previous PDF session found");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                RecentList.ItemsSource = RecentPDFs;
                return;
            }

            var result =RecentPDFs
     .Where(x => x.FileName.ToLower().Contains(keyword))
     .ToList();

            RecentList.ItemsSource = result;
        }
    }

   
}
