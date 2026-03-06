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

using System.Data.SqlClient;
using System.Security.Cryptography;



namespace self_studyhub
{
    /// <summary>
    /// Interaction logic for ForgotPasswordWindow.xaml
    /// </summary>
    public partial class ForgotPasswordWindow : Window
    {
        private string secretAnswerHash;
        private int userId;
        public ForgotPasswordWindow()
        {
            InitializeComponent();
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string answer = txtAnswer.Text.Trim();

            if (username == "")
            {
                MessageBox.Show("Enter your username", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=StudyControlDB;Integrated Security=True"))
                {
                    con.Open();

                    // Step 1: get secret question + hashed answer
                    string query = "SELECT id, SecretQuestion, SecretAnswerHash FROM Users_tb WHERE username=@username";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@username", username);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userId = Convert.ToInt32(reader["id"]);
                        lblSecretQuestion.Text = reader["SecretQuestion"].ToString();
                        secretAnswerHash = reader["SecretAnswerHash"].ToString();
                        reader.Close();

                        // Step 2: verify answer
                        string answerHash = ComputeSha256Hash(answer);
                        if (answerHash == secretAnswerHash)
                        {
                            // Step 3: generate temporary password
                            string tempPassword = GenerateTempPassword();

                            // Update password in DB
                            string updateQuery = "UPDATE Users_tb SET password=@tempPassword WHERE id=@id";
                            SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                            updateCmd.Parameters.AddWithValue("@tempPassword", tempPassword);
                            updateCmd.Parameters.AddWithValue("@id", userId);
                            updateCmd.ExecuteNonQuery();

                            MessageBox.Show($"Your temporary password: {tempPassword}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Secret answer is incorrect!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Username not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private string GenerateTempPassword()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random rnd = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }

        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
