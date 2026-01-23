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
using Microsoft.Web.WebView2.Wpf;
namespace self_studyhub.Pages
{
    /// <summary>
    /// Interaction logic for youtube.xaml
    /// </summary>
    public partial class YouTubePage : UserControl
    {
        
        public YouTubePage()
        {
            InitializeComponent();
            
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
        

        private async void LoadVideo_Click(object sender, RoutedEventArgs e)
        {
            if (VideoView.CoreWebView2 == null)
            {
                MessageBox.Show("WebView is still loading. Please wait.");
                return;
            }

            string url = youtubelinkbox.Text.Trim();

            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Please paste a YouTube link");
                return;
            }

            string embedUrl = ConvertToEmbedUrl(url);

            if (embedUrl == null)
            {
                MessageBox.Show("Invalid YouTube link");
                return;
            }

            // 3️⃣ Navigate YouTube embed
            VideoView.CoreWebView2.Navigate(embedUrl);
        }
        // 4️⃣ YouTube link → embed link
        private string ConvertToEmbedUrl(string url)
        {
            try
            {
                // https://www.youtube.com/watch?v=VIDEOID
                if (url.Contains("watch?v="))
                {
                    string videoId = url.Split(new[] { "v=" }, StringSplitOptions.None)[1]
                                        .Split('&')[0];
                    return $"https://www.youtube.com/embed/{videoId}";
                }

                // https://youtu.be/VIDEOID
                if (url.Contains("youtu.be/"))
                {
                    string videoId = url.Split(new[] { "youtu.be/" }, StringSplitOptions.None)[1]
                                        .Split('?')[0];
                    return $"https://www.youtube.com/embed/{videoId}";
                }

                // Already embed
                if (url.Contains("youtube.com/embed/"))
                {
                    return url;
                }
            }
            catch
            {
                return null;
            }

            return null;
        }
    }

}

