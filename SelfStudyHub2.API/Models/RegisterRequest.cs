namespace SelfStudyHub2.API.Models
{
    public class RegisterRequest
    {
        public string username { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public RegisterRequest()
        {
            username = "";
            email = "";
            password = "";
        }
    }
}
