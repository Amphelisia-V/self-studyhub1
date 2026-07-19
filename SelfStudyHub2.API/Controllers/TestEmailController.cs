using Microsoft.AspNetCore.Mvc;
using SelfStudyHub2.API.Services;

namespace SelfStudyHub2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestEmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public TestEmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> SendTestEmail()
        {
            await _emailService.SendEmailAsync(
                "phoothazinmyint042@gmail.com",
                "Self Study Hub Test",
                "Email sending works!"
            );

            return Ok("Email sent successfully");
        }
    }
}
