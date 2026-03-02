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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }
        private void Register_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Account Created Successfully!");

            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
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
            if (txtPassword.Visibility == Visibility.Visible)
            {
                txtPasswordVisible.Text = txtPassword.Password;
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordVisible.Visibility = Visibility.Visible;
                eyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOffOutline;
            }
            else
            {
                txtPassword.Password = txtPasswordVisible.Text;
                txtPassword.Visibility = Visibility.Visible;
                txtPasswordVisible.Visibility = Visibility.Collapsed;
                eyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOutline;
            }
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
