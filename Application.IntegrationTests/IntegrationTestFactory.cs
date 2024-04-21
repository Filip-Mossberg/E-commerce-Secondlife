using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;
using Testcontainers.RabbitMq;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using E_commerce.Context;
using MassTransit;
using AutoMapper;
using E_commerce.BLL.IService;
using E_commerce.BLL.Service.Consumer.CategoryConsumer;
using E_commerce.BLL.Service.Consumer.EmailConsumer;
using E_commerce.BLL.Service.Consumer.OrderConsumer;
using E_commerce.BLL.Service.Consumer.ProductConsumer;
using E_commerce.DAL.IRepository;
using E_commerce.Models;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;
using Azure.Storage.Blobs;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Application.IntegrationTests
{
    /// <summary>
    /// A factory class for integration tests that leverages WebApplicationFactory<Program> to create instances of the API.
    /// This class sets up new containers, modifies dependencies, and manages configurations to ensure a controlled testing environment.
    /// It implements the singleton design pattern to ensure that all tests share the same instance of the API, containers, and general configurations,
    /// thereby maintaining consistency across tests.
    /// </summary>
    public sealed class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private static IntegrationTestFactory _factory;
        private IntegrationTestFactory()
        {
      
        }

        public async static Task<IntegrationTestFactory> Instance()
        {
            if (_factory == null)
            {
                if (_factory == null)
                {
                    _factory = new IntegrationTestFactory();

                    await _factory.InitializeAsync();
                }
            }

            return _factory;
        }

        /// <summary>
        /// Configuring up containers for test environment 
        /// </summary>
        internal readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("ecommerce") 
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithHostname("database.test")
            .WithName("ecommerce.database.test")
            .WithExposedPort(5432) // Expose the internal port
            .WithPortBinding(5433, 5432) // Map internal port 5432 to external port 5433
            .WithNetwork("ecommerce-test")
            .Build();

        private readonly RedisContainer _redisContainer = new RedisBuilder()
            .WithImage("redis:latest")
            .WithHostname("redis.test")
            .WithName("ecommerce.redis.test")
            .WithExposedPort(6379)
            .WithPortBinding(6380, 6379)
            .WithNetwork("ecommerce-test")
            .Build();

        private readonly RabbitMqContainer _rabbitmqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:management")
            .WithHostname("rabbitmq.test")
            .WithName("ecommerce.rabbitmq.test")
            .WithExposedPort(5672)
            .WithPortBinding(5673, 5672)
            .WithExposedPort(15672)
            .WithPortBinding(15673, 15672)
            .WithNetwork("ecommerce-test")
            .WithUsername("mytestuser")
            .WithPassword("mytestpassword")
            .Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Setting the development to Testing
            builder.UseEnvironment("Testing");

            // Configuring dependencies for the test environment
            builder.ConfigureTestServices(services =>
            {

                // Removing existing DbContextOptions if exists and add a new instance
                var dbContextDescriptor = services
                    .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (dbContextDescriptor is not null)
                {
                    services.Remove(dbContextDescriptor);
                }

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseNpgsql(_dbContainer.GetConnectionString());
                });

                // Removing existing IPublishEndpont if exists and add a mocked version
                var descriptor2 = services
                    .SingleOrDefault(s => s.ServiceType == typeof(IPublishEndpoint));

                if (descriptor2 is not null)
                {
                    services.Remove(descriptor2);
                }

                var mockPublishEndpoint = new Mock<IPublishEndpoint>();

                services.AddSingleton<IPublishEndpoint>(mockPublishEndpoint.Object);

                // Configuring redis cache service
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = _redisContainer.GetConnectionString();
                });

                // Removing the current instance of BlobContainerClient while adding a Moq version to avoid uploading images to Azure blob storage.
                var BlobContainerClientDescriptor = services
                .FirstOrDefault(s => s.ServiceType == typeof(BlobContainerClient));

                if (BlobContainerClientDescriptor is not null)
                {
                    services.Remove(BlobContainerClientDescriptor);
                }

                var mockBlobContainerClient = new Mock<BlobContainerClient>();
                var mockBlobClient = new Mock<BlobClient>();
                var mockUri = new Uri("http://example.blob.core.windows.net/contaier/blob");

                mockBlobContainerClient
                    .Setup(c => c.GetBlobClient(It.IsAny<string>()))
                    .Returns(mockBlobClient.Object);

                mockBlobClient
                    .Setup(c => c.UploadAsync(It.IsAny<string>()));

                mockBlobClient
                    .Setup(c => c.Uri)
                    .Returns(mockUri);

                services.AddSingleton<BlobContainerClient>(mockBlobContainerClient.Object);


                // Configuring Message qeueue with mass transit (might not need this since I mock the queue any ways)
                services.AddMassTransit(options =>
                {
                    options.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(_rabbitmqContainer.Hostname, 5673, "/", h =>
                        {
                            h.Username("mytestuser");
                            h.Password("mytestpassword");
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
            });
        }

        // Starting containers
        public async Task InitializeAsync()
        {
            try
            {
                await Task.WhenAll(
                    _redisContainer.StartAsync(),
                    _rabbitmqContainer.StartAsync(),
                    _dbContainer.StartAsync()
                );
            }
            catch (Exception ex)
            {
                Log.Information($"Error initializing containers: {ex.Message}");
            }
        }

        // Stopping containers
        public new async Task DisposeAsync()
        {
            try
            {
                await _redisContainer.StopAsync();
                await _rabbitmqContainer.StopAsync();
                await _dbContainer.StopAsync();
            }
            catch (Exception ex)
            {
                Log.Information($"Error disposing containers: {ex.Message}");
            }
        }
    }
}