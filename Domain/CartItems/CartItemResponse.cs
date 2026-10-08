using E_Market.Server.Domain.Products;

namespace E_Market.Server.Domain.CartItems
{
    public record CartItemResponse(Guid Id, ProductResponse Product, int Quantity);
    
}
