using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfStudyHub2.API.Data;
using SelfStudyHub2.API.Models;

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
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            if (string.IsNullOrEmpty(request.username) ||
                string.IsNullOrEmpty(request.email) ||
                string.IsNullOrEmpty(request.password))
            {
                return BadRequest("All fields are required");
            }


            // Check email already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(x => x.email == request.email);


            if (existingUser != null)
            {
                return BadRequest("Email already exists");
            }


            User newUser = new User
            {
                username = request.username,
                email = request.email,
                password = request.password
            };


            _context.Users.Add(newUser);

            await _context.SaveChangesAsync();


            return Ok(new
            {
                message = "Register successful",
                userId = newUser.UserId
            });
        }
    }


    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
   
    }