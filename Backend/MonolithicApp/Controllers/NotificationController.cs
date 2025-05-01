using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace MonolithicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        // Simulate sending a notification
        [HttpPost]
        public IActionResult SendNotification([FromQuery] string mobile, [FromQuery] string message)
        {
            if (string.IsNullOrEmpty(mobile) || string.IsNullOrEmpty(message))
            {
                return BadRequest("Mobile number and message are required.");
            }

            // Simple validation for mobile number format (Example: Basic 10-digit number check)
            var phonePattern = @"^\d{10}$"; // This is a simple check for 10-digit numbers, adjust as per your format
            if (!Regex.IsMatch(mobile, phonePattern))
            {
                return BadRequest("Invalid mobile number format.");
            }

            // Simulating the notification sending logic (could be integrated with real services)
            var notificationStatus = SendSmsNotification(mobile, message); // Replace with actual service integration

            if (!notificationStatus)
            {
                return StatusCode(500, "Failed to send notification.");
            }

            return Ok(new { Message = $"Notification sent to {mobile}: {message}", Status = "Success" });
        }

        // Simulated method for sending SMS (replace with actual SMS service integration)
        private bool SendSmsNotification(string mobile, string message)
        {
            // Replace with actual logic to send SMS
            // For example, integrating with Twilio or another service
            return true; // Simulated success
        }
    }
}
