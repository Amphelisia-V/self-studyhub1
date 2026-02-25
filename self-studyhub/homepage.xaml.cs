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
using System.Collections.ObjectModel;

namespace self_studyhub.Pages
{
    /// <summary>
    /// Interaction logic for homepage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        ObservableCollection<string> tasks = new ObservableCollection<string>();
        public HomePage()
        {
            InitializeComponent();
            TaskListPanel.ItemsSource = tasks;

            // Example existing tasks
            tasks.Add("Watch 2 videos");
            tasks.Add("Take notes on chapter 3");
            tasks.Add("Practice coding exercise");
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string keyword = SearchBox.Text.ToLower();
            
            // နောက်ပိုင်း DB / List filter လုပ်လို့ရ
        }
        private void AddNewTask_Click(object sender, RoutedEventArgs e)
        {
            // Create a new TextBox for input
            TextBox newTaskBox = new TextBox
            {
                Width = 250,
                Margin = new Thickness(0, 5, 0, 0),
                Text = "Enter new task",
                Foreground = Brushes.Gray
            };

            // Placeholder simulation
            newTaskBox.GotFocus += (s, ev) =>
            {
                if (newTaskBox.Text == "Enter new task")
                {
                    newTaskBox.Text = "";
                    newTaskBox.Foreground = Brushes.White;
                }
            };
            newTaskBox.LostFocus += (s, ev) =>
            {
                if (string.IsNullOrWhiteSpace(newTaskBox.Text))
                {
                    newTaskBox.Text = "Enter new task";
                    newTaskBox.Foreground = Brushes.Gray;
                }
            };

            // Add task on Enter key
            newTaskBox.KeyDown += (s, args) =>
            {
                if (args.Key == Key.Enter && !string.IsNullOrWhiteSpace(newTaskBox.Text) && newTaskBox.Text != "Enter new task")
                {
                    tasks.Add(newTaskBox.Text);
                    TaskListPanel.Items.Remove(newTaskBox);
                }
            };

            // Add TextBox temporarily
            TaskListPanel.Items.Add(newTaskBox);
            newTaskBox.Focus();
        } 
    }
    }

