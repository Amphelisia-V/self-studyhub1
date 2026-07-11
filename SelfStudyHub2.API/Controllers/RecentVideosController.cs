using Microsoft.AspNetCore.Mvc;
using SelfStudyHub2.API.Data;
using SelfStudyHub2.API.Models;

namespace SelfStudyHub2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecentVideosController : ControllerBase
    {
        private readonly AppDbContext _context;


        public RecentVideosController(AppDbContext context)
        {
            _context = context;
        }



        // GET: api/RecentVideos/1
        [HttpGet("{userId}")]
        public IActionResult GetRecentVideos(int userId)
        {
            var videos = _context.RecentYTVideos
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.WatchedDate)
                .ToList();


            return Ok(videos);
        }



        // POST: api/RecentVideos
        [HttpPost]
        public IActionResult SaveRecentVideo(RecentVideo video)
        {

            video.WatchedDate = DateTime.Now;


            _context.RecentYTVideos.Add(video);

            _context.SaveChanges();


            return Ok(video);
        }
    }
}