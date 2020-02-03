using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data.Entities;
using Todo.Web.ViewModels;

namespace Todo.Services
{
    public interface ICreateUserService
    {
        Task<User> CreateUser(CreateUserViewModel createRequest);

    }
}
