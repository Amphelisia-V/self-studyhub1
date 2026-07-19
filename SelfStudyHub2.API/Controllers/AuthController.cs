
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SelfStudyHub2.API.Data;
using SelfStudyHub2.API.Models;
using SelfStudyHub2.API.Services;
using System.Security.Cryptography;

namespace SelfStudyHub2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly EmailService _emailService;

        public AuthController(AppDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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
                    x.email == request.Email );


            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }
          


            bool passwordCorrect = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.password
            );


            Console.WriteLine("Password Match: " + passwordCorrect);


            if (!passwordCorrect)
            {
                return Unauthorized("Invalid email or password");
            }
            if (!user.IsEmailVerified)
            {
                return Unauthorized("Please verify your email first");
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

            var otp = RandomNumberGenerator
     .GetInt32(100000, 999999)
     .ToString();

            var verification = new EmailVerification
            {
                Email = request.email,
                OTP = otp,
                ExpirationTime = DateTime.Now.AddMinutes(5),
                IsUsed = false
            };


            _context.EmailVerifications.Add(verification);

            await _context.SaveChangesAsync();


            await _emailService.SendEmailAsync(
                request.email,
                "Self Study Hub Email Verification",
                $"Your OTP Code is: {otp}"
            );


            return Ok(new
            {
                message = "OTP sent to your email"
            });
        }
        [HttpPost("verify-email-otp")]
        public async Task<IActionResult> VerifyEmailOTP(VerifyOTPRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) ||
   string.IsNullOrEmpty(request.OTP) ||
   string.IsNullOrEmpty(request.Username) ||
   string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("All fields required");
            }
            var verification = await _context.EmailVerifications
                .FirstOrDefaultAsync(x =>
                    x.Email == request.Email &&
                    x.OTP == request.OTP &&
                    !x.IsUsed);


            if (verification == null)
            {
                return BadRequest("Invalid OTP");
            }


            if (verification.ExpirationTime < DateTime.Now)
            {
                return BadRequest("OTP expired");
            }


            var user = new User
            {
                username = request.Username,
                email = request.Email,
                password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsEmailVerified = true,
                CreatedAt = DateTime.Now
            };


            _context.Users.Add(user);


            verification.IsUsed = true;


            await _context.SaveChangesAsync();


            return Ok(new
            {
                message = "Email verified and registration completed"
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
            string otp = RandomNumberGenerator
     .GetInt32(100000, 999999)
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

            await _emailService.SendEmailAsync(
                request.Email,
                "Password Reset OTP",
                $"Your OTP Code is: {otp}"
            );



            return Ok(new
            {
                message = "OTP sent to your email",
               
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
            reset.IsVerified = true;

            await _context.SaveChangesAsync();


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
     string.IsNullOrEmpty(request.OTP) ||
     string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest("Email OTP and Password required");
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
    x.OTP == request.OTP &&
    x.IsUsed == false &&
    x.IsVerified == true)
                 .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();



            if (reset == null)
            {
                return BadRequest("Please verify OTP first");
            }



            // Update password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            Console.WriteLine("New Password: " + request.NewPassword);
            Console.WriteLine("Hash: " + hashedPassword);

            user.password = hashedPassword;

            reset.IsUsed = true;

            await _context.SaveChangesAsync();


            return Ok(new
            {
                message = "Password reset successful"
            });
        }
    }

    public class ResetPasswordRequest
    {
        public string? Email { get; set; }
        public string? OTP { get; set; }
        public string? NewPassword { get; set; }
    }
    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
   
    }