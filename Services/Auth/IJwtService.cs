using E_Market.Server.Domain.Users;

namespace E_Market.Server.Services.Auth
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}