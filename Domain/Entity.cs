namespace E_Market.Server.Domain
{
    public class Entity
    {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid EditedBy { get; set; }
        public DateTime EditedOn { get; set; }

        public Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}
