using E_Market.Server.Domain.Products;

namespace E_Market.Server.Domain.CartItems
{
    public record CartItemRequest(Guid ProductId, int Quantity, Guid CartId);
    
}
