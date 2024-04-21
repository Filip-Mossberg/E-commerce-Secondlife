using AutoMapper;
using E_commerce.BLL.IService;
using E_commerce.BLL.Service;
using E_commerce.BLL.Service.Consumer.CategoryConsumer;
using E_commerce.BLL.Service.Consumer.EmailConsumer;
using E_commerce.BLL.Service.Consumer.OrderConsumer;
using E_commerce.BLL.Service.Consumer.ProductConsumer;
using E_commerce.BLL.Service.ServiceTest;
using E_commerce.BLL.Validation;
using E_commerce.DAL.IRepository;
using E_commerce.Models;
using E_commerce.Models.DTO_s.User;
using E_commerce_BLL.IService;
using E_commerce_BLL.Service;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_commerce_BLL
{
    public static class ConfigureServices
    {
        public static IServiceCollection DbServicesBLL(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            services.AddAutoMapper(typeof(MappingConfig));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddTransient<IRequestHandler<UserRegisterRequest, ApiResponse>, CreateUserService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(CreateCategoryService)));

            if (environment.EnvironmentName != "Testing")
            {
                services.AddStackExchangeRedisCache(redisOptions =>
                {
                    string redisConnection = configuration.GetConnectionString("Redis");
                    redisOptions.Configuration = redisConnection;
                });

                services.AddMassTransit(x =>
                {
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        var rabbitmqConnection = configuration.GetConnectionString("RabbitMQ");
                        cfg.Host(rabbitmqConnection, "/", h =>
                        {
                            h.Username("myuser");
                            h.Password("mypassword");
                        });

                        cfg.ReceiveEndpoint("category-created-event", e =>
                        {
                            e.Consumer(() => new CategoryCreateConsumer(
                                services.BuildServiceProvider().GetRequiredService<ICategoryRepository>(),
                                services.BuildServiceProvider().GetRequiredService<IMapper>()
                                ));
                        });

                        cfg.ReceiveEndpoint("category-update-event", e =>
                        {
                            e.Consumer(() => new CategoryUpdateConsumer(
                                services.BuildServiceProvider().GetRequiredService<ICategoryRepository>()
                                ));
                        });

                        cfg.ReceiveEndpoint("order-created-event", e =>
                        {
                            e.Consumer(() => new OrderCreateConsumer(
                                services.BuildServiceProvider().GetRequiredService<IOrderRepository>(),
                                services.BuildServiceProvider().GetRequiredService<IMapper>(),
                                services.BuildServiceProvider().GetRequiredService<IDistributedCache>(),
                                services.BuildServiceProvider().GetRequiredService<IProductRepository>()
                                ));
                        });

                        cfg.ReceiveEndpoint("email-send-event", e =>
                        {
                            e.Consumer(() => new EmailConsumer(
                                services.BuildServiceProvider().GetRequiredService<EmailConfiguration>()
                                ));
                        });

                        cfg.ReceiveEndpoint("product-create-event", e =>
                        {
                            e.Consumer(() => new ProductCreateConsumer(
                                services.BuildServiceProvider().GetRequiredService<IProductRepository>(),
                                services.BuildServiceProvider().GetRequiredService<IMapper>(),
                                services.BuildServiceProvider().GetRequiredService<IImageService>()
                                ));
                        });

                        cfg.ReceiveEndpoint("product-update-event", e =>
                        {
                            e.Consumer(() => new ProductUpdateConsumer(
                                services.BuildServiceProvider().GetRequiredService<IProductRepository>(),
                                services.BuildServiceProvider().GetRequiredService<IDistributedCache>()
                                ));
                        });
                    });
                });
            }

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
