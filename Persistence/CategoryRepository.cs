using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly AppDbContext _db;

        public CategoryRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task<bool> IsExistAsync(string categoryId, CancellationToken cancellationToken)
        {
            return await this._db.categories.AnyAsync(c => c.Id == categoryId, cancellationToken);
        }
    }
}
