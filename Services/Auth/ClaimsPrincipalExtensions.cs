using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace E_Market.Server.Services.Auth
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            string? id = principal.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

            if (Guid.TryParse(id, out Guid userId))
            {
                return userId;
            }

            throw new UnauthorizedAccessException("Token inválido ou usuário não identificado.");
        }
    }
}
