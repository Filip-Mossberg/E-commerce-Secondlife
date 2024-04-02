using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using E_commerce.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Testcontainers.PostgreSql;

namespace Application.IntegrationTests
{
    public class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        public ServiceProvider _serviceProvider;
        public IntegrationTestFactory()
        {
        }

        /// <summary>
        /// Configuring a Docker test container, containing PostgreSQL, along with a container for Redis and RabbitMQ
        /// </summary>
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("ecommerce") // Matches my database name configured in the connection string
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithName("ecommerce.database.test")
            .Build();

        private readonly IContainer _redisContainer = new ContainerBuilder()
            .WithImage("redis:latest")
            .WithName("ecommerce.redis.test")
            .Build();

        private readonly IContainer _rabbitmqContainer = new ContainerBuilder()
            .WithImage("rabbitmq:latest")
            .WithName("ecommerce.rabbitmq.test")
            .Build();

        // Starting containers and applying migrations
        public async Task InitializeAsync()
        {
            try
            {
                await _dbContainer.StartAsync();
                await _redisContainer.StartAsync();
                await _rabbitmqContainer.StartAsync();

                //var serviceColletion = new ServiceCollection();
                //_serviceProvider = serviceColletion.BuildServiceProvider();

                //var _context = _serviceProvider.GetRequiredService<AppDbContext>();
                //await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                Log.Information($"Error initializing containers: {ex.Message}");
            }
        }

        // Override the ConfigureWebHost method through WebApplicationFactory<Program> so we can set the environment and configurations if needed
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services
                    .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<AppDbContext>(options =>
                {
                    options
                        .UseNpgsql(_dbContainer.GetConnectionString());
                });
            });
        }

        // Stopping containers
        public new async Task DisposeAsync()
        {
            try
            {
                await _dbContainer.StopAsync();
                await _rabbitmqContainer.StopAsync();
                await _redisContainer.StopAsync();
            }
            catch (Exception ex)
            {
                Log.Information($"Error disposing containers: {ex.Message}");
            }
        }
    }
}