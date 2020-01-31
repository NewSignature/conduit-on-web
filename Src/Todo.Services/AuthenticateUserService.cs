using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data;
using Todo.Data.Entities;

namespace Todo.Services
{
    public class AuthenticateUserService
    {
        private readonly PasswordHashService _passwordHashService;
        private readonly IContext _context;

        public AuthenticateUserService(PasswordHashService passwordHashService, IContext context)
        {
            _context = context;
            _passwordHashService = passwordHashService;
        }

        public async Task<User> GetUserByUsernameAndPassword(string username, string password)
        {
            // first get the user by the username as given
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username.ToLower());
            if (user == null)
                return null;

            var hashedPassword = _passwordHashService.GetPasswordHash(password, user.Salt);
            if (string.Compare(user.Password, hashedPassword) != 0)
                return null;

            return user;
        }
    }
}
