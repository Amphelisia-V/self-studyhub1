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
using Microsoft.Web.WebView2.Core;
using Microsoft.VisualBasic;

namespace self_studyhub.Pages
{
    /// <summary>
    /// Interaction logic for browser.xaml
    /// </summary>
    public partial class BrowserPage : UserControl
    {
        public BrowserPage()
        {
            InitializeComponent();

            Loaded += BrowserPage_Loaded;

        }
        private async void BrowserPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize WebView2 ONLY
            await MyBrowser.EnsureCoreWebView2Async();

            // Hide browser at start
            MyBrowser.Visibility = Visibility.Collapsed;
            SearchPanel.Visibility = Visibility.Visible;
        }

        private void SearchBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            string query = SearchBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(query))
                return;

            // Switch UI
            SearchPanel.Visibility = Visibility.Collapsed;
            MyBrowser.Visibility = Visibility.Visible;

            // Navigate
            if (query.StartsWith("http"))
            {
                MyBrowser.Source = new Uri(query);
            }
            else
            {
                MyBrowser.Source = new Uri(
                    $"https://www.google.com/search?q={Uri.EscapeDataString(query)}");
            }
        }
        private void AddShortcut_Click(object sender, MouseButtonEventArgs e)
        {
            // Ask user for URL
            
        }
    }

}

