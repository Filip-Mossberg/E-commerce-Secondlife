using E_commerce.Models.DbModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E_commerce.Models.DbModels;
using E_commerce.Models;
using System.Reflection.Emit;

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
                .HasOne(u => u.User) // Cart has one user
                .WithOne(c => c.Cart) // User has one cart
                .HasForeignKey<Cart>(c => c.UserId); // Cart has the foreign key
            builder.Entity<Product>()
                .HasMany(c => c.Carts)
                .WithMany(p => p.Products);
        }

        public DbSet<Cart> Cart { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Order> Order { get; set; }
    }
}
