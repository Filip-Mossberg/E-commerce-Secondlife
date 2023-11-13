using E_commerce.Models.DTO_s.User;
using E_commerce_BLL.IService;
using E_commerce_BLL.Service;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace E_commerce_BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection DbServicesBLL(
            this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingConfig));

            services.AddScoped<IUserService, UserService>();
            services.AddValidatorsFromAssemblyContaining<UserUpdatePasswordRequest>();

            return services;
        }
    }
}
