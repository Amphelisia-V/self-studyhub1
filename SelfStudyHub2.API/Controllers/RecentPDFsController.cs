using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfStudyHub2.API.Data;
using SelfStudyHub2.API.Models;

namespace SelfStudyHub2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecentPDFsController : ControllerBase
    {
        private readonly AppDbContext _context;


        public RecentPDFsController(AppDbContext context)
        {
            _context = context;
        }



        // GET: api/RecentPDFs/1
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetRecentPDFs(int userId)
        {
            var pdfs = await _context.RecentPDFs
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OpenedDate)
                .ToListAsync();

            return Ok(pdfs);
        }
        // GET: api/RecentPDFs/last/1
        [HttpGet("last/{userId}")]
        public async Task<IActionResult> GetLastPDF(int userId)
        {
            var pdf = await _context.RecentPDFs
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OpenedDate)
                .FirstOrDefaultAsync();

            if (pdf == null)
                return NotFound();

            return Ok(pdf);
        }


        // POST: api/RecentPDFs
        [HttpPost]
        public async Task<IActionResult> SaveRecentPDF(RecentPDF pdf)
        {
            var existingPDF = await _context.RecentPDFs
                .FirstOrDefaultAsync(x =>
                    x.UserId == pdf.UserId &&
                    x.FilePath == pdf.FilePath);


            if (existingPDF != null)
            {
                existingPDF.FileName = pdf.FileName;
                existingPDF.OpenedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(existingPDF);
            }


            pdf.OpenedDate = DateTime.Now;
            pdf.IsSaved = true;


            _context.RecentPDFs.Add(pdf);

            await _context.SaveChangesAsync();


            return Ok(pdf);
        }
        [HttpPut("page")]
        public async Task<IActionResult> UpdatePage(RecentPDF pdf)
        {
            var existing = await _context.RecentPDFs
                .FindAsync(pdf.PdfId);

            if (existing == null)
            {
                return NotFound();
            }

            existing.CurrentPage = pdf.CurrentPage;

            await _context.SaveChangesAsync();

            return Ok(existing);
        }
    }
}