using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetRecentVideos(int userId)
        {
            var videos = await _context.RecentYTVideos
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.WatchedDate)
                .ToListAsync();

            return Ok(videos);
        }



        // POST: api/RecentVideos
        [HttpPost]
        public async Task<IActionResult> SaveRecentVideo(RecentVideo video)
        {
            var existingVideo = await _context.RecentYTVideos
                .FirstOrDefaultAsync(x =>
                    x.UserId == video.UserId &&
                    x.VideoUrl == video.VideoUrl);

            if (existingVideo != null)
            {
                existingVideo.WatchedDate = DateTime.Now;
                existingVideo.VideoTitle = video.VideoTitle;

                await _context.SaveChangesAsync();

                return Ok(existingVideo);
            }

            video.WatchedDate = DateTime.Now;

            _context.RecentYTVideos.Add(video);

            await _context.SaveChangesAsync();

            return Ok(video);
        }
    }
}