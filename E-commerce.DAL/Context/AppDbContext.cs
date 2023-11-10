using E_commerce.Models.DbModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E_commerce.Models;

namespace E_commerce.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Cart>()
                .HasOne(u => u.user) // Cart has one user
                .WithOne(c => c.cart) // User has one cart
                .HasForeignKey<Cart>(c => c.UserId); // Cart has the foreign key
        }

        public DbSet<Cart> Cart { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<ProductCart> ProductCart { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}
