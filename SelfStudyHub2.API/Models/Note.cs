using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfStudyHub2.API.Models
{
    [Table("Notes_tb")]
    public class Note
    {
        
       
            [Key]
            public int NoteId { get; set; }

            public string Title { get; set; }

            public string? Content { get; set; }

            public DateTime? Created { get; set; }

            public int UserId { get; set; }

        public Note()
        {
            Title = "";
        }
    }
}
