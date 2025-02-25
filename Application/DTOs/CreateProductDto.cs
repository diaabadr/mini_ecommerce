using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = "";

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StockQuanitty { get; set; }

        [Required]
        public string CategoryId { get; set; } = "";

    }
}
