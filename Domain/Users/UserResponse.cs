using E_Market.Server.Domain.Orders;
using E_Market.Server.Domain.Carts;

namespace E_Market.Server.Domain.Users
{
    public record UserResponse(Guid Id, string Name, string Email, List<Order> Orders, Cart cart);
    
}
