using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using E_commerce.Context;
using E_commerce_DAL.IRepository;
using E_commerce_DAL.Repository;
using E_commerce.Models.DbModels;
using E_commerce.DAL.IRepository;
using E_commerce.DAL.Repository;

namespace E_commerce_DAL
{
    public static class ConfigureServices
    {
        public static IServiceCollection DbServicesDAL(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("EcommerceDb")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(ProductRepository).Assembly));

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}


