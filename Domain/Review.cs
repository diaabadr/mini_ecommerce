using Domain.Entities;

namespace Domain
{
    public class Review
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Rate { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string UserId { get; set; }
        public User User { get; set; } = null!;

        public required string ProductId { get; set; }

        public Product Product { get; set; } = null!;

    }
}
