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

using Newtonsoft.Json;
using System.Net.Http;



namespace self_studyhub
{
    /// <summary>
    /// Interaction logic for ForgotPasswordWindow.xaml
    /// </summary>
    public partial class ForgotPasswordWindow : Window
    {
        private readonly HttpClient client = new HttpClient();

        private string userEmail = "";
        private string secretAnswerHash;
        private int userId;
        public ForgotPasswordWindow()
        {
            InitializeComponent();
        }

        private async void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            string email = userEmail;
            string password = txtNewPassword.Password.Trim();

            if (email == "" || password == "")
            {
                MessageBox.Show("Please fill all fields");
                return;
            }


            var data = new
            {
                email = email,
                newPassword = password
            };


            string json = JsonConvert.SerializeObject(data);


            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );


            try
            {
                var response = await client.PutAsync(
                    "https://localhost:7118/api/Auth/reset-password",
                    content
                );


                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show(
                        "Password reset successful"
                    );


                    LoginWindow login =
                        new LoginWindow();

                    login.Show();

                    this.Close();
                }
                else
                {
                    string error =
                        await response.Content.ReadAsStringAsync();

                    MessageBox.Show(error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private string GenerateTempPassword()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random rnd = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }

        private async void SendOTP_Click(object sender, RoutedEventArgs e)
        {
            userEmail = txtEmail.Text.Trim();


            if (userEmail == "")
            {
                MessageBox.Show("Enter email");
                return;
            }


            var data = new
            {
                email = userEmail
            };


            string json = JsonConvert.SerializeObject(data);


            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );


            var response = await client.PostAsync(
                "https://localhost:7118/api/Auth/forgot-password",
                content
            );


            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();

                dynamic otpResult = JsonConvert.DeserializeObject(result);

                MessageBox.Show(
                    "Your OTP is: " + otpResult.otp,
                    "OTP"
                );


                Step1.Visibility = Visibility.Collapsed;
                Step2.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Email not found");
            }
        }
        private async void VerifyOTP_Click(object sender, RoutedEventArgs e)
        {
            string otp = txtOTP.Text.Trim();


            if (otp == "")
            {
                MessageBox.Show("Enter OTP");
                return;
            }


            var data = new
            {
                email = userEmail,
                otp = otp
            };


            string json = JsonConvert.SerializeObject(data);


            var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );


            try
            {
                var response = await client.PostAsync(
                    "https://localhost:7118/api/Auth/verify-otp",
                    content
                );


                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("OTP verified");


                    Step2.Visibility = Visibility.Collapsed;
                    Step3.Visibility = Visibility.Visible;
                }
                else
                {
                    string error = await response.Content.ReadAsStringAsync();

                    MessageBox.Show(error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
