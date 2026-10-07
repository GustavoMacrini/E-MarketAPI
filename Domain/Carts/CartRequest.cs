using E_Market.Server.Domain.Products;

namespace E_Market.Server.Domain.Carts
{
    public record CartRequest(List<Product> Products);
    
}
