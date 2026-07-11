using MaterialDesignThemes.Wpf;
using Microsoft.Web.WebView2.Wpf;
using self_studyhub.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;


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

using Newtonsoft.Json;
using System.Net.Http;


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

            InitializeWebView();
            Loaded += async (s, e) =>
            {
                await LoadRecentVideos();
            };
            // WebView ကို စတင်ပွင့်ဖို့ ခေါ်ထားရပါမယ်

        }
        // 1️⃣ WebView2 ကို App start မှာ initialize
        private async void InitializeWebView()
        {
            try
            {
                await VideoView.EnsureCoreWebView2Async(null);


                VideoView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;
                VideoView.CoreWebView2.Settings.IsStatusBarEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task EnsureWebView()
        {
            if (VideoView.CoreWebView2 == null)
            {
                await VideoView.EnsureCoreWebView2Async();
            }
        }

        private async void LoadVideo_Click(object sender, RoutedEventArgs e)
        {
            await EnsureWebView();


            string url = youtubelinkbox.Text.Trim();


            string videoId = "";


            if (url.Contains("watch?v="))
            {
                videoId = url.Split(new[] { "v=" }, StringSplitOptions.None)[1]
                             .Split('&')[0];
            }
            else if (url.Contains("youtu.be"))
            {
                videoId = url.Split('/').Last()
                             .Split('?')[0];
            }


            if (string.IsNullOrEmpty(videoId))
            {
                MessageBox.Show("Invalid YouTube URL");
                return;
            }


            VideoView.CoreWebView2.Navigate(
     $"https://www.youtube-nocookie.com/embed/{videoId}"
 );
            await WatchVideo(
                "YouTube Video " + videoId,
                url
            );
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

            StringContent content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json");

            HttpResponseMessage response =
                await client.PostAsync(
                    "https://localhost:7118/api/RecentVideos",
                    content);

            if (response.IsSuccessStatusCode)
            {
                await LoadRecentVideos();
            }
            else
            {
                MessageBox.Show("Cannot save recent video");
            }
        }
        private async Task  LoadRecentVideos()
        {
            try
            {
                RecentVideos.Clear();

                HttpResponseMessage response =
                    await client.GetAsync(
                    $"https://localhost:7118/api/RecentVideos/{userId}"
                    );


                if (response.IsSuccessStatusCode)
                {
                    string json =
                        await response.Content.ReadAsStringAsync();


                    var videos =
                        JsonConvert.DeserializeObject<List<RecentVideo>>(json);


                    if (videos != null)
                    {
                        foreach (var video in videos)
                        {
                            RecentVideos.Add(video);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Cannot load recent videos");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void RecentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecentList.SelectedItem is RecentVideo video)
            {
                await EnsureWebView();

                string url = video.VideoUrl;

                string videoId = "";


                if (url.Contains("watch?v="))
                {
                    videoId = url.Split(new[] { "v=" }, StringSplitOptions.None)[1]
                                 .Split('&')[0];
                }
                else if (url.Contains("youtu.be"))
                {
                    videoId = url.Split('/').Last()
                                 .Split('?')[0];
                }


                if (!string.IsNullOrEmpty(videoId))
                {
                    VideoView.CoreWebView2.Navigate(
      $"https://www.youtube.com/embed/{videoId}?enablejsapi=1"
  );
                }
            }
        }
        public static event Action OnNoteSaved;
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

           
        }
        
       

    }

}

