using E_Market.Server.Domain.Categories;

namespace E_Market.Server.Domain.Products
{
    public record ProductResponse(Guid Id, string Name, CategoryResponse Category, string Description, decimal Price);
}
