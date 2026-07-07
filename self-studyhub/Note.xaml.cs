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
using System.Data.SqlClient;
using self_studyhub.Models;
using MaterialDesignThemes.Wpf;

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
            LoadNotes();
            YouTubePage.OnNoteSaved += LoadNotes;
            SortBox.SelectedIndex = 0;

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
            public int NoteId { get; set; }
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

            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();

                if (editingNote == null)
                {
                    // INSERT (New Note)
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Notes_tb (Title, Content, Created) VALUES (@Title, @Content, @Created)", con);

                    cmd.Parameters.AddWithValue("@Title", NoteTitleBox.Text);
                    cmd.Parameters.AddWithValue("@Content", NoteContentBox.Text);
                    cmd.Parameters.AddWithValue("@Created", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    // UPDATE (Existing Note)
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Notes_tb SET Title=@Title, Content=@Content WHERE NoteId=@NoteId", con);

                    cmd.Parameters.AddWithValue("@Title", NoteTitleBox.Text);
                    cmd.Parameters.AddWithValue("@Content", NoteContentBox.Text);
                    cmd.Parameters.AddWithValue("@NoteId", editingNote.NoteId);

                    cmd.ExecuteNonQuery();
                }

            }

            LoadNotes();
            CloseEditor_Click(null, null);
        }
            private void NoteTitleBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (NoteTitleBox.Text == "Untitled Note")
            {
                NoteTitleBox.Text = "";
            }
        }
        private void AddNoteCard(Note note)
        {
            Border card = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                CornerRadius = new CornerRadius(18),
                Margin = new Thickness(10),
                Padding = new Thickness(15),
                Cursor = Cursors.Hand
            };

            StackPanel stack = new StackPanel();

            // Title row (title + delete button)
            Grid topRow = new Grid();
            topRow.Margin = new Thickness(0, 0, 0, 8);

            topRow.ColumnDefinitions.Add(new ColumnDefinition());
            topRow.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });



            TextBlock titleBlock = new TextBlock
            {
                Text = note.Title,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(titleBlock, 0);

            Button deleteBtn = new Button
            {
                Width = 28,
                Height = 28,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(2),
                Cursor = Cursors.Hand,
                ToolTip = "Delete"
            };

            deleteBtn.Content = new PackIcon
            {
                Kind = PackIconKind.DeleteOutline,
                Width = 18,
                Height = 18,
                Foreground = Brushes.Gray
            };

            Grid.SetColumn(deleteBtn, 1);


            topRow.Children.Add(titleBlock);
            topRow.Children.Add(deleteBtn);

            TextBlock contentBlock = new TextBlock
            {
                Text = note.Content,
                Foreground = new SolidColorBrush(Color.FromRgb(120, 120, 120)),
                TextWrapping = TextWrapping.Wrap,
                MaxHeight = 50,
                Margin = new Thickness(0, 8, 0, 8)
            };

            TextBlock dateBlock = new TextBlock
            {
                Text = "🕒 " + note.Created.ToString("MMM dd yyyy"),
                FontSize = 11,
                Foreground = Brushes.Gray
            };

            stack.Children.Add(topRow);
            stack.Children.Add(contentBlock);
            stack.Children.Add(dateBlock);

            card.Child = stack;

            // ✏ Open edit
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

            // 🗑 DELETE CLICK
            deleteBtn.Click += async (s, e) =>
            {
                var dialog = new StackPanel
                {
                    Margin = new Thickness(20)
                };

                dialog.Children.Add(new PackIcon
                {
                    Kind = PackIconKind.AlertCircleOutline,
                    Width = 50,
                    Height = 50,
                    Foreground = Brushes.Orange,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                dialog.Children.Add(new TextBlock
                {
                    Text = "Delete this note?",
                    FontSize = 20,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 15, 0, 10),
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                dialog.Children.Add(new TextBlock
                {
                    Text = "This action cannot be undone.",
                    Foreground = Brushes.Gray,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                var buttons = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 20, 0, 0)
                };

                var cancelBtn = new Button
                {
                    Content = "Cancel",
                    Margin = new Thickness(5)
                };

                var deleteButton = new Button
                {
                    Content = "Delete",
                    Margin = new Thickness(5),
                    Background = Brushes.Red,
                    Foreground = Brushes.White
                };

                buttons.Children.Add(cancelBtn);
                buttons.Children.Add(deleteButton);

                dialog.Children.Add(buttons);

                cancelBtn.Click += (_, __) => DialogHost.CloseDialogCommand.Execute(false, null);
                deleteButton.Click += (_, __) => DialogHost.CloseDialogCommand.Execute(true, null);

                var result = await DialogHost.Show(dialog, "RootDialog");

                if ((bool)result)
                {
                    DeleteNote(note.NoteId);
                }

                e.Handled = true;
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
        public void AddNoteFromYoutube(string title, string content)
        {
            Note newNote = new Note
            {
                Title = title,
                Content = content,
                Created = DateTime.Now
            };

            notes.Add(newNote);
            AddNoteCard(newNote);
        }
        private void LoadNotes()
        {
            notes.Clear();

            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Notes_tb", con);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    notes.Add(new Note
                    {
                        NoteId = Convert.ToInt32(reader["NoteId"]),
                        Title = reader["Title"].ToString(),
                        Content = reader["Content"].ToString(),
                        Created = Convert.ToDateTime(reader["Created"])
                    });
                }
            }

            ApplySortAndSearch();
        }
        private void DeleteNote(int noteId)
        {
            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Notes_tb WHERE NoteId=@NoteId", con);

                cmd.Parameters.AddWithValue("@NoteId", noteId);
                cmd.ExecuteNonQuery();
            }

            LoadNotes();
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplySortAndSearch();
        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded) return;
            ApplySortAndSearch();
        }

        private void ApplySortAndSearch()
        {
            string keyword = SearchBox.Text?.ToLower() ?? "";

            var filtered = notes
                .Where(n =>
                    n.Title.ToLower().Contains(keyword) ||
                    n.Content.ToLower().Contains(keyword))
                .ToList();

            if (SortBox.SelectedIndex == 0)
            {
                filtered = filtered
                    .OrderByDescending(n => n.Created)
                    .ToList();
            }
            else
            {
                filtered = filtered
                    .OrderBy(n => n.Created)
                    .ToList();
            }

            NotesContainer.Children.Clear();

            foreach (var note in filtered)
            {
                AddNoteCard(note);
            }
        }
    }
}
