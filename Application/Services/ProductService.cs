using System;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Services;

public class ProductSoadService(AppDbContext dbContext) : IProductSoapService
{

    public async Task<string> CheckProductAvailability(string productId)
    {
        var isExist = await dbContext.products.AnyAsync(p => p.Id == productId);
        if (!isExist)
        {
            return "OUT OF STOCK";
        }

        return "IN STOCK";
    }
}