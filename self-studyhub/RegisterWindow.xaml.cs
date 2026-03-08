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
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace self_studyhub
{
   
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (username == "" || email == "" || password == "")
            {
                MessageBox.Show("Please fill all fields", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=StudyControlDB;Integrated Security=True"))
                {
                    con.Open();

                    // Check if username or email already exists
                    string checkQuery = "SELECT COUNT(*) FROM Users_tb WHERE username=@username OR email=@email";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@username", username);
                    checkCmd.Parameters.AddWithValue("@email", email);
                    int exists = (int)checkCmd.ExecuteScalar();

                    if (exists > 0)
                    {
                        MessageBox.Show("Username or Email already exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Insert new user
                    string query = "INSERT INTO Users_tb (username,email,password) VALUES (@username,@email,@password)";
                    SqlCommand cmd = new SqlCommand(query, con);
                   
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password); // optional: hash password later

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Account Created Successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        LoginWindow login = new LoginWindow();
                        login.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Registration failed!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
        private bool isVisible = false;

        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (isVisible)
            {
                txtPassword.Visibility = Visibility.Visible;
                txtPasswordVisible.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordVisible.Visibility = Visibility.Visible;
            }
            isVisible = !isVisible; // toggle
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Fade in
            DoubleAnimation fade = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1.5));
            SignUpPanel.BeginAnimation(OpacityProperty, fade);

            // Slide up
            DoubleAnimation slide = new DoubleAnimation(40, 0, TimeSpan.FromSeconds(1));
            slide.EasingFunction = new QuadraticEase()
            {
                EasingMode = EasingMode.EaseOut
            };

            TranslateTransform transform = (TranslateTransform)SignUpPanel.RenderTransform;
            transform.BeginAnimation(TranslateTransform.YProperty, slide);
        }
        private void OpenRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            register.Show();
            this.Close();
        }
    }
}
