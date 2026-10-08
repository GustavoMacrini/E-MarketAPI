using E_Market.Server.Domain.Products;

namespace E_Market.Server.Domain.CartItems
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Product Product{ get; set; }
        public int Quantity { get; set; }

        public CartItem() { }
        public CartItem(Product product, int quantity)
        {
            Id = Guid.NewGuid();
            Product = product;
            Quantity = quantity;
        }
    }
}
