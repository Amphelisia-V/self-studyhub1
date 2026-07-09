using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SelfStudyHub2.API.Models
{
    [Table("Users_tb")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string? username { get; set; }

        public string? email { get; set; }

        public string? password { get; set; }
    }
}
