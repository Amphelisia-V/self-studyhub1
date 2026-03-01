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
            if (!isVisible)
            {
                txtPasswordVisible.Text = txtPassword.Password;
                txtPassword.Visibility = Visibility.Collapsed;
                txtPasswordVisible.Visibility = Visibility.Visible;

                eyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOffOutline;
                isVisible = true;
            }
            else
            {
                txtPassword.Password = txtPasswordVisible.Text;
                txtPassword.Visibility = Visibility.Visible;
                txtPasswordVisible.Visibility = Visibility.Collapsed;

                eyeIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.EyeOutline;
                isVisible = false;
            }
        }
    }  
    }

