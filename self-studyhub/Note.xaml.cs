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
            editingNote = null;

            NoteTitleBox.Text = "Untitled Note";
            NoteContentBox.Text = "";

            EditorOverlay.Visibility = Visibility.Visible;

            var slideIn = new DoubleAnimation
            {
                From = 600,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300)
            };

            EditorTransform.BeginAnimation(TranslateTransform.XProperty, slideIn);
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
        public class Note
        {
            public string Title { get; set; }
            public string Content { get; set; }
            public DateTime Created { get; set; }
        }

        private List<Note> notes = new List<Note>();
        private Note editingNote = null;
    
    private void SaveNote_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NoteTitleBox.Text))
                return;

            if (editingNote == null)
            {
                // Create new note
                Note newNote = new Note
                {
                    Title = NoteTitleBox.Text,
                    Content = NoteContentBox.Text,
                    Created = DateTime.Now
                };

                notes.Add(newNote);
                AddNoteCard(newNote);
            }
            else
            {
                // Update existing note
                editingNote.Title = NoteTitleBox.Text;
                editingNote.Content = NoteContentBox.Text;

                RefreshNotes();
            }

            CloseEditor_Click(null, null);
        }
        private void AddNoteCard(Note note)
        {
            Border card = new Border
            {
                Background = Brushes.White,
                CornerRadius = new CornerRadius(18),
                Margin = new Thickness(10),
                Padding = new Thickness(15),
                Cursor = Cursors.Hand
            };

            StackPanel stack = new StackPanel();

            TextBlock titleBlock = new TextBlock
            {
                Text = note.Title,
                FontSize = 16,
                FontWeight = FontWeights.Bold
            };

            TextBlock contentBlock = new TextBlock
            {
                Text = note.Content,
                Foreground = Brushes.Gray,
                TextWrapping = TextWrapping.Wrap,
                MaxHeight = 50
            };

            TextBlock dateBlock = new TextBlock
            {
                Text = "🕒 " + note.Created.ToString("MMM dd yyyy"),
                FontSize = 11,
                Foreground = Brushes.LightGray
            };

            stack.Children.Add(titleBlock);
            stack.Children.Add(contentBlock);
            stack.Children.Add(dateBlock);

            card.Child = stack;

            card.MouseLeftButtonUp += (s, e) =>
            {
                editingNote = note;
                NoteTitleBox.Text = note.Title;
                NoteContentBox.Text = note.Content;

                EditorOverlay.Visibility = Visibility.Visible;

                var slideIn = new DoubleAnimation
                {
                    From = 600,
                    To = 0,
                    Duration = TimeSpan.FromMilliseconds(300)
                };

                EditorTransform.BeginAnimation(TranslateTransform.XProperty, slideIn);
            };

            NotesContainer.Children.Add(card);
        }
        private void RefreshNotes()
        {
            NotesContainer.Children.Clear();

            foreach (var note in notes)
            {
                AddNoteCard(note);
            }
        }
        public void AddNoteFromYoutube(string content)
        {
            Note newNote = new Note
            {
                Title = content.Split('\n')[0],
                Content = content,
                Created = DateTime.Now
            };

            notes.Add(newNote);
            AddNoteCard(newNote);
        }
    }
}
