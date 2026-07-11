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
        private int userID;
        HomePage homePage ;
        YouTubePage youTubePage;
        BrowserPage browserPage = new BrowserPage();
        PDFPage pDFPage;
        NotePage note;
        public PDFPage PDFPageInstance { get; private set; }
        public MainWindow(int userID)
        {
            InitializeComponent();

            this.userID = userID;

            homePage = new HomePage(userID);

            note = new NotePage(userID);

            youTubePage = new YouTubePage(userID);

            pDFPage = new PDFPage(userID);

            PDFPageInstance = pDFPage;


            YouTubePage.OnNoteSaved += note.LoadNotes;


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
