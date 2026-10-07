using E_Market.Server.Domain.Carts;
using E_Market.Server.Domain.Orders;
using E_Market.Server.Domain.Products;
using Microsoft.AspNetCore.Http.HttpResults;

namespace E_Market.Server.Domain.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Order>? Orders { get; set; }
        public Cart Cart { get; set; }

        public User() { }
        public User(string name, string email, string password, List<Order>? orders, Cart cart)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            Password = password;
            Orders = orders;
            Cart = cart;

        }
    }
}
