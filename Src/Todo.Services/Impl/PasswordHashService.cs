using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Services.Impl
{
    public class PasswordHashService
    {
        private const int SALT_SIZE = 10;

        public (string, string) HashPassword(string rawPassword)
        {
            var salt = Encoding.UTF8.GetString(CreateRandomSalt(SALT_SIZE));
            var hashedPassword = GetPasswordHash(rawPassword, salt);

            return (hashedPassword, salt);
        }

        public string GetPasswordHash(string rawPassword, string salt)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(rawPassword + "-" + salt);
            using (var sha256Hash = SHA256.Create())
            {
                var hashedBytes = sha256Hash.ComputeHash(passwordBytes);
                return Encoding.UTF8.GetString(hashedBytes);
            }
        }

        byte[] CreateRandomSalt(int saltSize)
        {
            byte[] randomBytes = new byte[saltSize];
            RNGCryptoServiceProvider crytoProvider = new RNGCryptoServiceProvider();
            crytoProvider.GetBytes(randomBytes);

            return randomBytes;
        }
    }
}
