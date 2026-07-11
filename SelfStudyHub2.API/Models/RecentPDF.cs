using System.ComponentModel.DataAnnotations;

namespace SelfStudyHub2.API.Models
{
    public class RecentPDF
    {
        public int PdfId { get; set; }

        public int UserId { get; set; }

        public string FileName { get; set; } = "";

        public string FilePath { get; set; } = "";

        public bool IsSaved { get; set; }

        public DateTime OpenedDate { get; set; }

        public int? CurrentPage { get; set; }
    }
}
