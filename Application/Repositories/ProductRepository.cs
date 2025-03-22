using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        public ProductRepository(AppDbContext db, IMapper mapper)
        {
            this._db = db;
            this._mapper = mapper;
        }

        public async Task<Product> AddProductAsync(Product product, string supplierId, CancellationToken cancellationToken)
        {
            _db.products.Add(product);
            var productSupplier = new ProductSupplier { ProductId = product.Id, UserId = supplierId };
            await _db.ProductSuppliers.AddAsync(productSupplier);
            await _db.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task<int> UpdateProductAsync(string id, Product product, CancellationToken cancellationToken)
        {
            var existingProduct = await _db.products.FindAsync(id);
            if (existingProduct == null)
            {
                return 0;
            }

            // _mapper.Map(product, existingProduct);
            existingProduct.Name = product.Name;

            var affected = await _db.SaveChangesAsync(cancellationToken);
            return affected == 0 ? 1 : affected;
        }

        public async Task<int> DeleteProductAsync(string Id, CancellationToken cancellationToken)
        {

            var product = await this._db.products.FindAsync(Id);
            if (product == null)
            {
                return 0;
            }

            this._db.Remove(product);
            return await this._db.SaveChangesAsync();
        }


    }
}
