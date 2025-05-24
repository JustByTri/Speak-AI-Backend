namespace DAL.Entities
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
    }

}