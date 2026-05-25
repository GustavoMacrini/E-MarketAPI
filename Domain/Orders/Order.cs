using E_Market.Server.Domain.Products;

namespace E_Market.Server.Domain.Orders
{
    public class Order : Entity
    {
        public List<Product> Products { get; set; }

        public Order()
        {
            
        }

        public Order(List<Product> products)
        {
            Products = products;
        }
    }
}
