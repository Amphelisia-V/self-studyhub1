namespace SelfStudyHub2.API.Models
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }

        public string NewPassword { get; set; }


        public ResetPasswordRequest()
        {
            Email = "";
            NewPassword = "";
        }
    }
}
