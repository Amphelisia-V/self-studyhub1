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
using System.Net.Http;
using Newtonsoft.Json;

namespace self_studyhub
{
    /// <summary>
    /// Interaction logic for VerifyOTPWindow.xaml
    /// </summary>
    public partial class VerifyOTPWindow : Window
    {
        private readonly HttpClient client = new HttpClient();


        private string username;
        private string email;
        private string password;
        public VerifyOTPWindow(string username,
    string email,
    string password)
        {
            InitializeComponent();

            this.username = username;
            this.email = email;
            this.password = password;
        }

        private async void Verify_Click(object sender, RoutedEventArgs e)
        {
            string otp = txtOTP.Text.Trim();


            if (otp == "")
            {
                MessageBox.Show("Enter OTP");
                return;
            }


            try
            {

                var verifyData = new
                {
                    email = email,
                    otp = otp,
                    username = username,
                    password = password
                };


                string json = JsonConvert.SerializeObject(verifyData);


                StringContent content =
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json"
                    );


                HttpResponseMessage response =
                    await client.PostAsync(
                    "https://localhost:7118/api/Auth/verify-email-otp",
                    content);


                if (response.IsSuccessStatusCode)
                {

                    MessageBox.Show(
                        "Registration completed successfully"
                    );


                    LoginWindow login = new LoginWindow();
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
    }
}
