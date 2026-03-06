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
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
       
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (email == "" || password == "")
            {
                MessageBox.Show("Please enter username and password", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=StudyControlDB;Integrated Security=True"))
                {
                    con.Open();

                    string query = "SELECT id, username, email FROM Users_tb WHERE email=@Email AND password=@password";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@password", password);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int userId =Convert.ToInt32(reader["id"]);
                        string userName = reader["username"].ToString();
                        string emailFromDb = reader["email"].ToString(); // rename to avoid conflict

                        MessageBox.Show($"Welcome {userName}!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Open HomePage (UserControl or Window)
                        MainWindow main = new MainWindow(userId);
                        main.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Username or password incorrect!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Visibility == Visibility.Visible)
            {
                txtPasswordVisible.Text = txtPassword.Password;

                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordVisible.Visibility = Visibility.Visible;

                txtPasswordVisible.Focus();
                txtPasswordVisible.CaretIndex = txtPasswordVisible.Text.Length;

                eyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOffOutline;
            }
            else
            {
                txtPassword.Password = txtPasswordVisible.Text;

                txtPasswordVisible.Visibility = Visibility.Collapsed;
                txtPassword.Visibility = Visibility.Visible;

                txtPassword.Focus();

                eyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOutline;
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Fade in
            DoubleAnimation fade = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(2));
            loginPanel.BeginAnimation(OpacityProperty, fade);

            // Slide up
            DoubleAnimation slide = new DoubleAnimation(40, 0, TimeSpan.FromSeconds(1));
            slide.EasingFunction = new QuadraticEase()
            {
                EasingMode = EasingMode.EaseOut
            };

            TranslateTransform transform = (TranslateTransform)loginPanel.RenderTransform;
            transform.BeginAnimation(TranslateTransform.YProperty, slide);
        }
        private void OpenRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            register.Show();
            this.Close();
        }

        private void Forgotpw_Click(object sender, RoutedEventArgs e)
        {
            ForgotPasswordWindow forgotWindow = new ForgotPasswordWindow();
            forgotWindow.ShowDialog();
        }
    }  
    }

