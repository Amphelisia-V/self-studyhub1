using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SelfStudyHub2.API.Models
{
    [Table("Users_tb")]
    public class User
    {
        public int UserId { get; set; }

        public string username { get; set; } = string.Empty;

        public string email { get; set; } = string.Empty;

        public string password { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public bool IsEmailVerified { get; set; }
    }
}
