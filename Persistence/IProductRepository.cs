using Domain.Entities;

namespace Persistence
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProducts();

        Task<Product?> GetByIdAsync(string Id);
        Task<Product> AddProductAsync(Product product);

        Task<int> UpdateProductAsync(string id, Product product);

        Task<int> DeleteProductAsync(string id);
    }
}
