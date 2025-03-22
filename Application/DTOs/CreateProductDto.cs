
namespace Application.DTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; } = "";

        public decimal Price { get; set; }

        public int StockQuanitty { get; set; }

        public string CategoryId { get; set; } = "";
    }
}
