namespace E_Market.Server.Domain.Categories
{
    public class Category : Entity
    {
        public string Name { get; private set; }
        public bool Active { get; private set; }

        public Category(string name, string createdBy, string editedBy)
        {
            Name = name;
            Active = true;
            CreatedBy = createdBy;
            EditedBy = editedBy;
            CreatedOn = DateTime.UtcNow;
            EditedOn = DateTime.UtcNow;
        }
    }
}
