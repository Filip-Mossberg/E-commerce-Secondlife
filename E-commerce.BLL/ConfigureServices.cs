using E_commerce.BLL.IService;
using E_commerce.BLL.Service;
using E_commerce.BLL.Validation;
using E_commerce.Models.DTO_s.User;
using E_commerce_BLL.IService;
using E_commerce_BLL.Service;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace E_commerce_BLL
{
    public static class ConfigureServices
    {
        public static IServiceCollection DbServicesBLL(
            this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingConfig));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IImageService, ImageService>();

            services.AddValidatorsFromAssemblyContaining<UserRegisterValidation>();
            services.AddValidatorsFromAssemblyContaining<UserUpdatePasswordValidation>();
            services.AddValidatorsFromAssemblyContaining<CategoryCreateValidation>();
            services.AddValidatorsFromAssemblyContaining<ImageUploadValidation>();

            return services;
        }
    }
}
