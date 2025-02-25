using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public required string Name { get; set; }

        public decimal Price { get; set; }

        public int StockQuanitty { get; set; }

        [ForeignKey("Category")]
        public string CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
