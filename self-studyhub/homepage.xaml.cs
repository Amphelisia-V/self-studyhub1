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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace self_studyhub.Pages
{
    public partial class HomePage : UserControl
    {
        private DispatcherTimer timer;
        private TimeSpan timeLeft;
        private bool isRunning = false;

        public HomePage()
        {
            InitializeComponent();
            UpdateProgress(39);
            timeLeft = TimeSpan.FromMinutes(25);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;// Example
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
        
           
            private void Timer_Tick(object sender, EventArgs e)
            {
                if (timeLeft.TotalSeconds > 0)
                {
                    timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));
                    TimerText.Text = timeLeft.ToString(@"mm\:ss");
                }
                else
                {
                    timer.Stop();
                    MessageBox.Show("Time's up!");
                    StartButton.Content = "START";
                    isRunning = false;
                }
            }

            private void StartTimer(object sender, RoutedEventArgs e)
            {
                if (!isRunning)
                {
                    timer.Start();
                    StartButton.Content = "STOP";
                    isRunning = true;
                }
                else
                {
                    timer.Stop();
                    StartButton.Content = "START";
                    isRunning = false;
                }
            }
        private void ResetTimer(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timeLeft = TimeSpan.FromMinutes(25);
            TimerText.Text = timeLeft.ToString(@"mm\:ss");
        }
    }
    }



    




