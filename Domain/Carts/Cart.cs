using E_Market.Server.Domain.Products;

namespace E_Market.Server.Domain.Carts
{
    public class Cart : Entity
    {
        public List<Product> Products { get; set; }
        public Cart()
        {
            Products = new List<Product>();
        }
        public Cart(List<Product> products, string createdBy)
        {
            Products = products;
            CreatedBy = createdBy;
            EditedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            EditedOn = DateTime.UtcNow;
        }
    }
}
