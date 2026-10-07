using E_Market.Server.Domain.Orders;
using E_Market.Server.Domain.Carts;

namespace E_Market.Server.Domain.Users
{
    public record UserRequest(string Name, string Email, string Password);
    
}
