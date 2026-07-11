using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelfStudyHub2.API.Models
{
    public class RecentVideo
    {
        public int RecentVideoId { get; set; }

        public int UserId { get; set; }

        public string VideoTitle { get; set; } = "";

        public string VideoUrl { get; set; } = "";

        public string? ThumbnailUrl { get; set; } 

        public DateTime WatchedDate { get; set; }


        
    }
}
