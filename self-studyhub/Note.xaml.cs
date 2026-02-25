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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace self_studyhub.Pages
{
    /// <summary>
    /// Interaction logic for Note.xaml
    /// </summary>
    public partial class NotePage : UserControl
    {
        public NotePage()
        {
            InitializeComponent();
        }
        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            EditorOverlay.Visibility = Visibility.Visible;

            var slideIn = new DoubleAnimation
            {
                From = 600,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300)
            };

            EditorTransform.BeginAnimation(TranslateTransform.XProperty, slideIn);

            NoteTitleBox.Text = "Untitled Note";
            NoteContentBox.Text = "";
        }

        private void CloseEditor_Click(object sender, RoutedEventArgs e)
        {
            var slideOut = new DoubleAnimation
            {
                From = 0,
                To = 600,
                Duration = TimeSpan.FromMilliseconds(300)
            };

            slideOut.Completed += (s, _) =>
            {
                EditorOverlay.Visibility = Visibility.Collapsed;
            };

            EditorTransform.BeginAnimation(TranslateTransform.XProperty, slideOut);
        }
    }
}
