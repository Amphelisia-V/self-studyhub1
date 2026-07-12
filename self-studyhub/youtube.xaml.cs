using MaterialDesignThemes.Wpf;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;
using self_studyhub.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
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
using static self_studyhub.Pages.PDFPage;
using System.Net.Http;
using Newtonsoft.Json;

namespace self_studyhub.Pages
{
    /// <summary>
    /// Interaction logic for youtube.xaml
    /// </summary>
    public partial class YouTubePage : UserControl
    {
        public ObservableCollection<RecentVideo> RecentVideos { get; set; }

        private readonly HttpClient client = new HttpClient();

        private int userId;
        public YouTubePage(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            SaveSnackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
            RecentVideos = new ObservableCollection<RecentVideo>();

            RecentList.ItemsSource = RecentVideos;

            this.Loaded += YouTubePage_Loaded;

            // WebView ကို စတင်ပွင့်ဖို့ ခေါ်ထားရပါမယ်
        
        }
       
        // 1️⃣ WebView2 ကို App start မှာ initialize
        private async Task InitializeWebView()
        {
            try
            {
                await VideoView.EnsureCoreWebView2Async(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("WebView2 initialization error:\n" + ex.Message);
            }
        }
        private async void YouTubePage_Loaded(object sender, RoutedEventArgs e)
        {
            await InitializeWebView();

            await LoadRecentVideos();
        }

        private async void LoadVideo_Click(object sender, RoutedEventArgs e)
        {
            if (VideoView != null && VideoView.CoreWebView2 != null)
            {
                await VideoView.EnsureCoreWebView2Async();

                string url = youtubelinkbox.Text;

                if (url.Contains("watch?v="))
                {
                    string videoId = url.Split(new[] { "v=" }, StringSplitOptions.None)[1].Split('&')[0];
                   
                    string embedUrl = $"https://www.youtube.com/embed/{videoId}";
                    
                    VideoView.CoreWebView2.Navigate(embedUrl);

                    VideoView.CoreWebView2.NavigationCompleted += async (s, args) =>
                    {
                        string title = await VideoView.CoreWebView2.ExecuteScriptAsync(
                            "document.title"
                        );

                        title = title.Replace("\"", "");

                        await WatchVideo(title, url);
                    };
                }
                else if (url.StartsWith("https://"))
                {
                    VideoView.CoreWebView2.Navigate(url);

                    VideoView.CoreWebView2.NavigationCompleted += async (s, args) =>
                    {
                        string title = await VideoView.CoreWebView2.ExecuteScriptAsync(
                            "document.title"
                        );

                        title = title.Replace("\"", "");

                        // Add Recent Video
                        await WatchVideo(title, url);
                    };
                }

            }
            else
            {
                MessageBox.Show("please wait a second");
            }
        }
        private async Task WatchVideo(string title, string url)
        {
            var video = new RecentVideo
            {
                UserId = userId,
                VideoTitle = title,
                VideoUrl = url,
                WatchedDate = DateTime.Now
            };


            string json = JsonConvert.SerializeObject(video);


            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );


            var response = await client.PostAsync(
                "https://localhost:7118/api/RecentVideos",
                content
            );


            if (response.IsSuccessStatusCode)
            {
                await LoadRecentVideos();
            }
            else
            {
                MessageBox.Show("Save video failed");
            }
        }
        private void RecentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecentList.SelectedItem is RecentVideo video)
            {
                if (VideoView != null && VideoView.CoreWebView2 != null)
                {
                    string url = video.VideoUrl;

                    if (url.Contains("watch?v="))
                    {
                        string videoId = url.Split(new[] { "v=" }, StringSplitOptions.None)[1]
                                            .Split('&')[0];

                        string embedUrl = $"https://www.youtube.com/embed/{videoId}";

                        VideoView.CoreWebView2.Navigate(embedUrl);
                    }
                    else
                    {
                        VideoView.CoreWebView2.Navigate(url);
                    }
                }
            }
        }
        private async Task LoadRecentVideos()
        {
            try
            {
                var response = await client.GetAsync(
                    $"https://localhost:7118/api/RecentVideos/{userId}"
                );

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    var videos = JsonConvert.DeserializeObject<List<RecentVideo>>(json);

                    RecentVideos.Clear();

                    if (videos != null)
                    {
                        foreach (var video in videos)
                        {
                            RecentVideos.Add(video);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public event Action<string, string> NoteSaved;
        private void SaveNote_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                MessageBox.Show("Please enter a note title.");
                return;
            }

            if (string.IsNullOrWhiteSpace(NoteBox.Text))
            {
                MessageBox.Show("Please enter a note.");
                return;
            }

            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Notes_tb (UserId, Title, Content, Created) VALUES (@UserId, @Title, @Content, @Created)", con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Title", TitleBox.Text);
                cmd.Parameters.AddWithValue("@Content", NoteBox.Text);
                cmd.Parameters.AddWithValue("@Created", DateTime.Now);

                cmd.ExecuteNonQuery();
            }

            SaveSnackbar.MessageQueue?.Enqueue("✅ Note saved successfully!");

            TitleBox.Clear();
            NoteBox.Clear();
            OnNoteSaved?.Invoke();
        }
        public static event Action OnNoteSaved;


    }

}

