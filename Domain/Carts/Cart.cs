using E_Market.Server.Domain.CartItems;
using E_Market.Server.Domain.Products;

namespace E_Market.Server.Domain.Carts
{
    public class Cart : Entity
    {
        public List<CartItem> CartItems { get; set; }
        public Cart()
        {
            
        }
        public Cart(List<CartItem> cartItems, string createdBy)
        {
            CartItems = cartItems;
            CreatedBy = createdBy;
            EditedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            EditedOn = DateTime.UtcNow;
        }
    }
}
