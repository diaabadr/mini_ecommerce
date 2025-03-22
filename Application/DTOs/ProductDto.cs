using System;
namespace Domain.Entities;

public class ProductDto
{
    public required string Id { get; set; }

    public required string Name { get; set; }

    public decimal Price { get; set; }

    public int StockQuanitty { get; set; }

    public Category? Category { get; set; }

    public ICollection<SupplierDto> Suppliers { get; set; } = [];

    public UserDto? Creator { get; set; }

}

public class SupplierDto
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class UserDto
{
    public string Id { get; set; }

    public string Name { get; set; }
}