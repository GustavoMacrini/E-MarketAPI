using E_Market.Server.Domain.Products;

namespace E_Market.Server.Domain.Carts
{
    public record CartResponse(Guid Id, List<Product>? Products);
    
}
