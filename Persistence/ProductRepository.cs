using AutoMapper;
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

        public async Task<List<Product>> GetAllProducts()
        {
            return await this._db.products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(string Id)
        {
            return await this._db.products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == Id);
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            this._db.products.Add(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task<int> UpdateProductAsync(string id, Product product)
        {
            //Console.Write("product id", product.Id);
            var existingProduct = await this._db.products.FindAsync(id);
            if (existingProduct == null)
            {
                return 0;
            }

            _mapper.Map(product, existingProduct);

            var affected = await this._db.SaveChangesAsync();
            return affected == 0 ? 1 : affected;
        }

        public async Task<int> DeleteProductAsync(string Id)
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
