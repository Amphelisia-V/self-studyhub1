using System;

namespace SelfStudyHub2.API.Models
{
    public class PasswordReset
    {
        public int ResetId { get; set; }

        public int UserId { get; set; }

        public string OTP { get; set; }

        public DateTime ExpireTime { get; set; }

        public bool IsUsed { get; set; }

        public DateTime CreatedDate { get; set; }


        public PasswordReset()
        {
            OTP = "";
        }
    }
}
