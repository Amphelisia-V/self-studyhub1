using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace self_studyhub.Pages
{
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
            UpdateProgress(39); // Example
        }
        private void UpdateProgress(double percent)
        {
            double maxWidth = 300;
            ProgressFill.Width = (percent / 100) * maxWidth;
            ProgressTitle.Text = $"{percent}% to complete";
        }

        private void OpenTaskModal(object sender, RoutedEventArgs e)
        {
            TaskModalOverlay.Visibility = Visibility.Visible;
        }

        private void CloseTaskModal(object sender, RoutedEventArgs e)
        {
            TaskModalOverlay.Visibility = Visibility.Collapsed;
        }

        private void SaveTask(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskNameBox.Text))
            {
                TaskList.Items.Add(TaskNameBox.Text);
                TaskNameBox.Clear();
                TaskModalOverlay.Visibility = Visibility.Collapsed;
            }
        }

        private void DeleteTask(object sender, RoutedEventArgs e)
        {
            if (TaskList.SelectedItem != null)
            {
                TaskList.Items.Remove(TaskList.SelectedItem);
            }
        }
    }
}

    




