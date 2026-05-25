using E_Market.Server.Domain.Categories;

namespace E_Market.Server.Domain.Products
{
    public record ProductRequest(string Name, Guid CategoryId, string Description, decimal Price);

}
