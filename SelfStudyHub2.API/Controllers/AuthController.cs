using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfStudyHub2.API.Data;

namespace SelfStudyHub2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (request.Email == null || request.Password == null)
            {
                return BadRequest("Email and Password required");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x =>
                    x.email == request.Email &&
                    x.password == request.Password);


            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }


            return Ok(new
            {
                userId = user.UserId,
                username = user.username,
                email = user.email
            });

        }
    }


    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}