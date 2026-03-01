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

namespace self_studyhub
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        bool isVisible = false;
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string Email = txtEmail.Text;

            // 👇 Visible ဖြစ်နေရင် TextBox ထဲကယူမယ်
            string password = txtPassword.Visibility == Visibility.Visible
                                ? txtPassword.Password
                                : txtPasswordVisible.Text;

            if (Email == "admin" && password == "1234")
            {
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Login");
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
    }  
    }

