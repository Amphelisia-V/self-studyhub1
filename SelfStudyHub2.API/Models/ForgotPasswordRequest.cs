namespace SelfStudyHub2.API.Models
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }

        public ForgotPasswordRequest()
        {
            Email = "";
        }
    }
}
