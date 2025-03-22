using System;
using Domain.Entities;

namespace Domain;

public class ProductSupplier
{
    public string? UserId { get; set; }

    public User User { get; set; } = null!;

    public string? ProductId { get; set; }

    public Product Product { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}