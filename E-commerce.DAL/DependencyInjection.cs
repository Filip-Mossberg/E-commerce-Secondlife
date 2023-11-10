using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using E_commerce.Context;
using E_commerce_DAL.IRepository;
using E_commerce_DAL.Repository;
using E_commerce.Models.DbModels;

namespace E_commerce_DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection DbServicesDAL(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("EcommerceDb")));

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}


