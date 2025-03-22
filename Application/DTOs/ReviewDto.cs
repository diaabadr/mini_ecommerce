namespace Application.DTOs
{
    public class ReviewDto
    {
        public required string Id { get; set; }
        public required int Rate { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public required string UserId { get; set; }
        public required string DisplayName { get; set; }
    }
}
