using Domain.Entities;

namespace Persistence
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts(CancellationToken cancellationToken);

        Task<Product?> GetByIdAsync(string Id, CancellationToken cancellationToken);
        Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken);

        Task<int> UpdateProductAsync(string id, Product product, CancellationToken cancellationToken);

        Task<int> DeleteProductAsync(string id, CancellationToken cancellationToken);
    }
}
