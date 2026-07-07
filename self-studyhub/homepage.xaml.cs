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
using System.Data.SqlClient;
using self_studyhub.Models;

namespace self_studyhub.Pages
{
    
    public partial class HomePage : UserControl
    {
        private DispatcherTimer timer;
        private TimeSpan timeLeft;
        private bool isRunning = false;

        private int userId;
        private int focusMinutes = 25;

        public HomePage(int userId)
        {
            InitializeComponent();
            
            DrawCircleProgress(0.6);
            UpdateDailyGoal(3, 15);
            Loaded += HomePage_Loaded;
            this.userId = userId;
            LoadTasks();
        

        timeLeft = TimeSpan.FromMinutes(25);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            UpdateTimerUI();
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
            if (string.IsNullOrWhiteSpace(TaskNameBox.Text))
                return;

            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();

                string query = @"INSERT INTO Tasks_tb(UserId, Title, IsCompleted)
                         VALUES(@UserId, @Title, 0)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Title", TaskNameBox.Text);

                cmd.ExecuteNonQuery();
            }

            TaskNameBox.Clear();
            TaskModalOverlay.Visibility = Visibility.Collapsed;

            
            LoadTasks();
        }

        private void DeleteTask(object sender, RoutedEventArgs e)
        {
            if (TaskList.SelectedItem != null)
            {
                TaskList.Items.Remove(TaskList.SelectedItem);

                _total = TaskList.Items.Count;

                if (_completed > _total)
                    _completed = _total;

                UpdateDailyGoal(_completed, _total);
            }
        }


        // ================= TIMER LOGIC =================

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timeLeft.TotalSeconds > 0)
            {
                timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));
                UpdateTimerUI();

                // 🔥 Last 10 sec sound
                if (timeLeft.TotalSeconds <= 10 && timeLeft.TotalSeconds > 0)
                {
                    Console.Beep();
                }
            }
            else
            {
                timer.Stop();

                // 🔔 Finish sound
                Console.Beep();

                TimerText.Text = "00:00";
                StartButton.Content = "START";
                TimerText.Foreground = Brushes.White;
                isRunning = false;

                MessageBox.Show("Time's up! 🎉");
            }
        }

        private void StartTimer(object sender, RoutedEventArgs e)
        {
            if (!isRunning)
            {
                // ⬇ Read user input
                if (!int.TryParse(TimeInput.Text, out focusMinutes) || focusMinutes <= 0)
                {
                    MessageBox.Show("Enter valid minutes!");
                    return;
                }
                if(focusMinutes>300)
               {
               MessageBox.Show("maximum is 300 minutes");
               return;
               }

                // ⬇ Set time based on input
                timeLeft = TimeSpan.FromMinutes(focusMinutes);

                timer.Start();
                StartButton.Content = "PAUSE";
                isRunning = true;
            }
            else
            {
                timer.Stop();
                StartButton.Content = "RESUME";
                isRunning = false;
            }

            UpdateTimerUI();
        }

        private void ResetTimer(object sender, RoutedEventArgs e)
        {
            timer.Stop();

            timeLeft = TimeSpan.FromMinutes(focusMinutes);

            UpdateTimerUI();

            StartButton.Content = "START";
            TimerText.Foreground = Brushes.White;
            isRunning = false;
        }

        // ================= UI UPDATE =================

        private void UpdateTimerUI()
        {
            TimerText.Text = timeLeft.ToString(@"hh\:mm\:ss");

            // 🔴 Last 5 min color change
            if (timeLeft.TotalMinutes <= 5)
            {
                TimerText.Foreground = Brushes.Red;
            }
            else
            {
                TimerText.Foreground = Brushes.White;
            }

            // 🔥 OPTIONAL: Circle Progress (if you use DrawCircleProgress)
            double totalSeconds = focusMinutes*60; // 25 min
            double percent = timeLeft.TotalSeconds / totalSeconds;
           
        }
        private void Task_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            TaskItem task = checkBox.DataContext as TaskItem;

            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();

                string query = "UPDATE Tasks_tb SET IsCompleted = 1 WHERE TaskId = @TaskId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@TaskId", task.TaskId);

                cmd.ExecuteNonQuery();
            }

            LoadTasks();
            ShowMessage("Great Job 🎉", "Task Completed!");
        }

        private void Task_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            TaskItem task = checkBox.DataContext as TaskItem;

            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();

                string query = "UPDATE Tasks_tb SET IsCompleted = 0 WHERE TaskId = @TaskId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@TaskId", task.TaskId);

                cmd.ExecuteNonQuery();
            }

            LoadTasks();
            ShowMessage("Updated", "Task marked incomplete.");
        }
        private void DrawCircleProgress(double percentage)
        {
            double angle = percentage * 360;
            double radius = 60;
            Point center = new Point(70, 70);

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
            if (total == 0)
            {
                ProgressScale.ScaleX = 0;
                DailyGoalText.Text = "0/0 tasks completed";
                DailyGoalInfo.Text = "No tasks yet";
                return;
            }

            double percentage = (double)completed / total;
            ProgressScale.ScaleX = percentage;

            DailyGoalText.Text = $"{completed}/{total} tasks completed";

            int left = total - completed;
            DailyGoalInfo.Text = $"{left} tasks left to complete today's goal";
        }
        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTasks();
        }
        
        private int _completed = 0;
        private int _total = 0;

        private void DailyGoalProgressContainer_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateDailyGoal(_completed, _total);
        }
        private void ShowMessage(string title, string message)
        {
            MessageTitle.Text = title;
            MessageText.Text = message;
            CustomMessage.Visibility = Visibility.Visible;
        }

        private void CloseCustomMessage(object sender, RoutedEventArgs e)
        {
            CustomMessage.Visibility = Visibility.Collapsed;
        }
        private List<TaskItem> tasks = new List<TaskItem>();
        private void LoadTasks()
        {
            tasks.Clear();

            using (SqlConnection con = new SqlConnection(DatabaseHelper.ConnectionString))
            {
                con.Open();

                string query = "SELECT * FROM Tasks_tb WHERE UserId=@UserId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tasks.Add(new TaskItem
                    {
                        TaskId = Convert.ToInt32(reader["TaskId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Title = reader["Title"].ToString(),
                        IsCompleted = Convert.ToBoolean(reader["IsCompleted"])
                    });
                }
            }

            TaskList.ItemsSource = null;
            TaskList.ItemsSource = tasks;

            _total = tasks.Count;
            _completed = tasks.Count(t => t.IsCompleted);

            UpdateDailyGoal(_completed, _total);
        }

    }
}



    




