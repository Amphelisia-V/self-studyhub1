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
using System.Net.Http;
using Newtonsoft.Json;

namespace self_studyhub
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly HttpClient client = new HttpClient();
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
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
                var loginData = new
                {
                    Email = email,
                    Password = password
                };


                string json = JsonConvert.SerializeObject(loginData);


                StringContent content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );


                HttpResponseMessage response = await client.PostAsync(
                    "https://localhost:7118/api/Auth/login",
                    content
                );


                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    LoginResponse login = JsonConvert.DeserializeObject<LoginResponse>(result);

                    if (login == null)
                    {
                        MessageBox.Show("Login response is invalid.");
                        return;
                    }

                    MainWindow main = new MainWindow(login.UserId);
                    main.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        "Email or Password Incorrect!",
                        "Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
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
        public class LoginResponse
        {
            public int UserId { get; set; }

            public string Username { get; set; }

            public string Email { get; set; }
            public LoginResponse()
            {
                Username = "";
                Email = "";
            }
        }
    }  
    }

