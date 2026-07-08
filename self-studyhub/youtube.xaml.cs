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
using System.Collections.ObjectModel;

namespace self_studyhub.Pages
{
    /// <summary>
    /// Interaction logic for youtube.xaml
    /// </summary>
    public partial class YouTubePage : UserControl
    {
        public ObservableCollection<RecentVideo> RecentVideos { get; set; }
        public YouTubePage()
        {
            InitializeComponent();
            SaveSnackbar.MessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));
            RecentVideos = new ObservableCollection<RecentVideo>();

            RecentList.ItemsSource = RecentVideos;

            // WebView ကို စတင်ပွင့်ဖို့ ခေါ်ထားရပါမယ်
            InitializeWebView();
        }
        // 1️⃣ WebView2 ကို App start မှာ initialize
        private async void InitializeWebView()
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
        

        private void LoadVideo_Click(object sender, RoutedEventArgs e)
        {
           if(VideoView != null && VideoView.CoreWebView2 !=null)
            {
                string url = youtubelinkbox.Text;
                if (url.Contains("watch?v="))
                {
                    string videoId = url.Split(new[] { "v=" }, StringSplitOptions.None)[1].Split('&')[0];
                    string embedUrl = $"https://www.youtube.com/embed/{videoId}";
                    VideoView.CoreWebView2.Navigate(embedUrl);

                    WatchVideo("YouTube Video " + videoId, url);
                }
                else if (url.StartsWith("https://"))
                {
                    VideoView.CoreWebView2.Navigate(url);

                    // Add Recent Video
                    WatchVideo(url, url);
                }

            }else
            {
                MessageBox.Show("please wait a second");
            }
        }
        private void WatchVideo(string title, string url)
        {
            RecentVideos.Insert(0, new RecentVideo
            {
                Title = title,
                VideoUrl = url,
                WatchedDate = DateTime.Now
            });
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
        public event Action<string,string> NoteSaved;
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
                    "INSERT INTO Notes_tb (Title, Content, Created) VALUES (@Title, @Content, @Created)", con);

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

