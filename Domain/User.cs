using System;
using System.Text.Json.Serialization;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Domain;
public class User : IdentityUser
{
    public string? DisplayName { get; set; }
    public string? Bio { get; set; }

    public string? ImageUrl { get; set; }

    public ICollection<ProductSupplier> Products { get; set; } = [];
}