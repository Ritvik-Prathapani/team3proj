using MonolithicApp.Data;
using MonolithicApp.Helpers;
using MonolithicApp.Models;
using System.Linq;

namespace MonolithicApp.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public AuthService(AppDbContext context, JwtTokenHelper jwtTokenHelper)
        {
            _context = context;
            _jwtTokenHelper = jwtTokenHelper;
        }

        public string AuthenticateUser(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                return null;
            }

            return _jwtTokenHelper.GenerateJwtToken(user);
        }
    }
}
