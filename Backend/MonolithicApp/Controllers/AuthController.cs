using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MonolithicApp.Data;
using MonolithicApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System;
using System.Security.Claims;

namespace MonolithicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly string _secretKey = "your-secret-key"; // Replace with your secret key

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            // Find user by username and password
            var foundUser = _context.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

            // If user is not found, return Unauthorized
            if (foundUser == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            // Generate JWT token for authenticated user
            var token = GenerateJwtToken(foundUser);

            return Ok(new { token });  // Return the token in the response
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)  // Add Role as a claim
            };

            var token = new JwtSecurityToken(
                issuer: "your-app",  // Replace with your app's name or URL
                audience: "your-app", // Replace with your app's name or URL
                claims: claims,
                expires: DateTime.Now.AddHours(1), // Token expiration time
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);  // Return JWT token as string
        }
    }
}
