using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        // products is the db table name
        // registering the product entity as table in db
        public DbSet<Product> products { get; set; }

        public DbSet<Category> categories { get; set; }





    }
}
