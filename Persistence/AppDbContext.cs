using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
    {
        public DbSet<Product> products { get; set; }

        public DbSet<Category> categories { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public required DbSet<ProductSupplier> ProductSuppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProductSupplier>(x => x.HasKey(a => new { a.ProductId, a.UserId }));

            builder.Entity<ProductSupplier>().HasOne(x => x.User).WithMany(x => x.Products).
            HasForeignKey(x => x.UserId);

            builder.Entity<ProductSupplier>().HasOne(x => x.Product).WithMany(x => x.Suppliers).
            HasForeignKey(x => x.ProductId);
        }

    }
}
