using E_Market.Server.Domain.Carts;
using E_Market.Server.Domain.Orders;
using Microsoft.AspNetCore.Identity;

namespace E_Market.Server.Domain.Users
{
    public class User : IdentityUser<Guid>
    {        
        public string Name { get; set; }
        public List<Order>? Orders { get; set; }
        public Cart Cart { get; set; }

        public User() { }
        public User(string name, string email, List<Order>? orders, Cart cart)
        {            
            Name = name;
            Orders = orders;
            Cart = cart;
            Email = email;
            UserName = email;
        }
    }
}
