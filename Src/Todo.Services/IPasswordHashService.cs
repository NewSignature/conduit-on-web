using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Services
{
    public interface IPasswordHashService
    {
        (string, string) HashPassword(string rawPassword);
        string GetPasswordHash(string rawPassword, string salt);
    }
}
