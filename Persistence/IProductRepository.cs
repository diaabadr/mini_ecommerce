using Domain.Entities;

namespace Persistence
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product, string supplierId, CancellationToken cancellationToken);
        Task<int> UpdateProductAsync(string id, Product product, CancellationToken cancellationToken);
        Task<int> DeleteProductAsync(string id, CancellationToken cancellationToken);
    }
}
