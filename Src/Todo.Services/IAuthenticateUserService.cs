using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data.Entities;

namespace Todo.Services
{
    public interface IAuthenticateUserService
    {
        Task<User> GetUserByUsernameAndPassword(string username, string password);
    }
}
