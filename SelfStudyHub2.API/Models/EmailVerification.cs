using System.ComponentModel.DataAnnotations;

namespace SelfStudyHub2.API.Models
{
    public class EmailVerification
    {
        [Key]
        public int VerificationId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string OTP { get; set; } = string.Empty;

        public DateTime ExpirationTime { get; set; }

        public bool IsUsed { get; set; }
    }
}
