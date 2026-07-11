using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfStudyHub2.API.Data;
using SelfStudyHub2.API.Models;

namespace SelfStudyHub2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> SaveNote(Note note)
        {
            if (string.IsNullOrWhiteSpace(note.Title))
            {
                return BadRequest("Title is required.");
            }

            note.Created = DateTime.Now;

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Note saved successfully.",
                noteId = note.NoteId
            });
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetNotes(int userId)
        {
            var notes = await _context.Notes
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.Created)
                .ToListAsync();

            return Ok(notes);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.NoteId == id);

            if (note == null)
            {
                return NotFound("Note not found");
            }

            _context.Notes.Remove(note);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Note deleted successfully"
            });
        }
    }



}