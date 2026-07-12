namespace SelfStudyHub2.API.Models
{
    public class VerifyOTPRequest
    {
        public string Email { get; set; }

        public string OTP { get; set; }


        public VerifyOTPRequest()
        {
            Email = "";
            OTP = "";
        }
    }
}
