using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Common;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Web.ViewModels;

namespace Todo.Services
{
    public class CreateUserService
    {
        private readonly IContext _context;
        private readonly PasswordHashService _passwordHashService;

        public CreateUserService(PasswordHashService passwordHashService, IContext context)
        {
            _passwordHashService = passwordHashService;
            _context = context;
        }

        public async Task<User> CreateUser(CreateUserViewModel createRequest)
        {
            // does another user have the same username?
            var existingUserForUsername = await _context.Users.FirstOrDefaultAsync(x => x.Username == createRequest.Username.ToLower());
            if (existingUserForUsername != null)
                throw new DuplicateUserException();

            var (hashedPassword, salt) = _passwordHashService.HashPassword(createRequest.Password);
            var newUser = new User
            {
                Username = createRequest.Username.ToLower(),
                Password = hashedPassword,
                Salt = salt,
                FirstName = createRequest.FirstName,
                LastName = createRequest.LastName
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }
    }
}
