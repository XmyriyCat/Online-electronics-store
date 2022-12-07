using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using AdminPanel.ModelsDb;
using System.Text;

namespace AdminPanel.Models
{
    public class Authorization
    {
        private readonly WarehouseContext context;
        public Authorization(WarehouseContext context)
        {
            this.context = context;
        }
        public bool IsAuthorized(string login, string password)
        {
            Manager? sourceManager = context.Managers
                .Where(m => m.Login.Equals(login))
                .FirstOrDefault();

            if (sourceManager is null)
            {
                return false;
            }

            byte[] hashSalt = Encoding.UTF8.GetBytes(sourceManager.Salt);

            string hashedPassword = GenerateHashHMACSHA256(password, hashSalt);

            if (sourceManager.HashPassword.Equals(hashedPassword))
            {
                return true;
            }

            return false;
        }

        public string GenerateHashHMACSHA256(string password, byte[] salt)
        {
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashedPassword;
        }

        public string GetUserName(string login)
        {
            string userName = context.Managers
                .Where(m => m.Login.Equals(login))
                .Select(s => s.UserName)
                .FirstOrDefault() ?? string.Empty;

            return userName;
        }
    }
}
