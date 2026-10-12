using Microsoft.AspNetCore.Identity;

namespace E_Market.Server.Services.Password
{
    public class PasswordService
    {
        private readonly PasswordHasher<object> _hasher;

        public PasswordService()
        {

        }

        public string HashPassword(string password)
        {
            return _hasher.HashPassword(null, password);
        }
    }
}
