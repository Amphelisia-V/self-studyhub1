
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
        //forgot password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Email required");
            }


            // Check user exists
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.email == request.Email);


            if (user == null)
            {
                return NotFound("Email not found");
            }



            // Generate 6 digit OTP
            Random random = new Random();

            string otp = random.Next(100000, 999999)
                .ToString();



            // Create password reset record

            PasswordReset reset = new PasswordReset
            {
                UserId = user.UserId,

                OTP = otp,

                ExpireTime = DateTime.Now.AddMinutes(5),

                IsUsed = false,

                CreatedDate = DateTime.Now
            };



            _context.PasswordResets.Add(reset);

            await _context.SaveChangesAsync();



            return Ok(new
            {
                message = "OTP generated",
                otp = otp
            });
        }
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOTP(VerifyOTPRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.OTP))
            {
                return BadRequest("Email and OTP required");
            }


            // Find user
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.email == request.Email);


            if (user == null)
            {
                return NotFound("Email not found");
            }



            // Check OTP
            var reset = await _context.PasswordResets
                .Where(x =>
                    x.UserId == user.UserId &&
                    x.OTP == request.OTP &&
                    x.IsUsed == false)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();



            if (reset == null)
            {
                return BadRequest("Invalid OTP");
            }



            // Check expiry
            if (reset.ExpireTime < DateTime.Now)
            {
                return BadRequest("OTP expired");
            }



            return Ok(new
            {
                message = "OTP verified",
                userId = user.UserId
            });
        }
        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) ||
                string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest("Email and new password required");
            }


            // Find user
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.email == request.Email);


            if (user == null)
            {
                return NotFound("Email not found");
            }



            // Find verified OTP
            var reset = await _context.PasswordResets
                .Where(x =>
                    x.UserId == user.UserId &&
                    x.IsUsed == false)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();



            if (reset == null)
            {
                return BadRequest("Please verify OTP first");
            }



            // Update password
            user.password = request.NewPassword;



            // Mark OTP as used
            reset.IsUsed = true;



            await _context.SaveChangesAsync();



            return Ok(new
            {
                message = "Password reset successful"
            });
        }
    }


    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
   
    }