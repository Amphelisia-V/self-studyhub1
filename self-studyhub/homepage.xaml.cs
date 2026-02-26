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
            DrawCircleProgress(0.6);
            UpdateDailyGoal(3, 15);
            Loaded += HomePage_Loaded;


            timeLeft = TimeSpan.FromMinutes(25);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;// Example
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
            StartButton.Content = "START";
            isRunning = false;
        }
        private void Task_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Task Completed!");
        }

        private void Task_Unchecked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Task Unchecked!");
        }
        private void DrawCircleProgress(double percentage)
        {
            double angle = percentage * 360;
            double radius = 70;
            Point center = new Point(80, 80);

            double radians = (Math.PI / 180) * (angle - 90);
            double x = center.X + radius * Math.Cos(radians);
            double y = center.Y + radius * Math.Sin(radians);

            bool isLargeArc = angle > 180;

            PathFigure figure = new PathFigure();
            figure.StartPoint = new Point(center.X, center.Y - radius);

            ArcSegment arc = new ArcSegment();
            arc.Point = new Point(x, y);
            arc.Size = new Size(radius, radius);
            arc.IsLargeArc = isLargeArc;
            arc.SweepDirection = SweepDirection.Clockwise;

            figure.Segments.Add(arc);

            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            ProgressArc.Data = geometry;
        }
        private void UpdateDailyGoal(int completed, int total)
        {
            double percentage = (double)completed / total;

            // Bar width change
            DailyGoalBar.Width = percentage * 300; // 300 = progress bar container width

            // Text change
            DailyGoalText.Text = $"{completed}/{total} tasks completed";

            int left = total - completed;
            DailyGoalInfo.Text = $"{left} tasks left to complete today's goal";
            DailyGoalBar.Width = percentage * ((Grid)DailyGoalBar.Parent).ActualWidth;
        }
        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDailyGoal(3, 15);
        }
    }
    }



    




