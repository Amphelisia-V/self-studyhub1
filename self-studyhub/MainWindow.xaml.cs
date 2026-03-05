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
using self_studyhub.Pages;
using Microsoft.Web.WebView2.Core;

namespace self_studyhub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HomePage homePage = new HomePage();
        YouTubePage youTubePage = new YouTubePage();
        BrowserPage browserPage = new BrowserPage();
        PDFPage pDFPage = new PDFPage();
        note note = new note();
        public MainWindow()
        {
            InitializeComponent();

            MainContent.Content = homePage;

        }
      
       

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = homePage;
        }

        private void YouTubeButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = youTubePage;
        }

        private void BrowserButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = browserPage;

           
        }

        private void PDFButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = pDFPage;

        }

        private void NoteButton_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = note;
        }

        private void PackIcon_ColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {

        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked!");
        }
    }
}
