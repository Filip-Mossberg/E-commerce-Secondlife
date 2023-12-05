using E_commerce.BLL.IService;
using E_commerce.BLL.Service;
using E_commerce.BLL.Validation;
using E_commerce.Models;
using E_commerce.Models.DTO_s.User;
using E_commerce_BLL.IService;
using E_commerce_BLL.Service;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_commerce_BLL
{
    public static class ConfigureServices
    {
        public static IServiceCollection DbServicesBLL(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MappingConfig));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddValidatorsFromAssemblyContaining<UserRegisterValidation>();

            // Email Config
            var emailConfig = configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            return services;
        }
    }
}
