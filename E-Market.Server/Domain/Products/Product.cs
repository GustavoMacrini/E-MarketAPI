using E_Market.Server.Domain.Categories;
using E_Market.Server.Domain.Orders;
using Microsoft.AspNetCore.Http.HttpResults;

namespace E_Market.Server.Domain.Products
{
    public class Product : Entity
    {
        public string Name { get; private set; }
        public Category Category { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; set; }
        public bool Active { get; private set; } = true;
        public ICollection<Order> Orders { get; set;}


        public Product() { }

        public Product(string name, Category category, string description, decimal price, string createdBy)
        {
            Name = name;
            Category = category;
            Description = description;
            Price = price;

            CreatedBy = createdBy;
            EditedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            EditedOn = DateTime.UtcNow;
        }

        public void EditValues(string name, Category category, string description, decimal price, string editedBy)
        {
            Name = name;
            Category = category;
            Description = description;
            Price = price;

            EditedBy = editedBy;
            CreatedOn = DateTime.UtcNow;
            EditedOn = DateTime.UtcNow;

        }
    }
}
