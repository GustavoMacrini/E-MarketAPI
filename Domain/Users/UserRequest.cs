using System.ComponentModel.DataAnnotations;

namespace E_Market.Server.Domain.Users
{
    public record UserRequest(string Name, [EmailAddress]string Email, string Password);
    
}
