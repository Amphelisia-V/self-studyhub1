using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
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
           if(VideoView != null && VideoView.CoreWebView2 !=null)
            {
                string url = youtubelinkbox.Text;
                if (url.Contains("watch?v="))
                {
                    string videoId = url.Split(new[] { "v=" }, StringSplitOptions.None)[1].Split('&')[0];
                    string embedUrl = $"https://www.youtube.com/embed/{videoId}";
                    VideoView.CoreWebView2.Navigate(embedUrl);
                }
                else if (url.StartsWith("https://"))
                {
                    VideoView.CoreWebView2.Navigate(url);
                }

            }else
            {
                MessageBox.Show("please wait a second");
            }
        }
    }

}

