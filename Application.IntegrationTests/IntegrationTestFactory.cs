using E_commerce.Context;
using E_commerce.Models.DbModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Testcontainers.PostgreSql;

namespace Application.IntegrationTests
{
    public class IntegrationTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private ServiceProvider _serviceProvider;
        public IntegrationTestFactory()
        {
        }

        /// <summary>
        /// Configuring a Docker test container, containing PostgreSql
        /// </summary>
        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("EcommerceDb")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        // Starting database container and applying migrations
        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();

            var serviceColletion = new ServiceCollection();

            // Initialize _serviceProvider
            _serviceProvider = serviceColletion.BuildServiceProvider();

            var _context = _serviceProvider.GetRequiredService<AppDbContext>();
            await _context.Database.MigrateAsync();
        }

        // Configuring the ASP .NET Core web host
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<AppDbContext>));
                var descriptor2 = services.SingleOrDefault(s => s.ServiceType == typeof(IdentityUser));

                var _serviceCollection = new ServiceCollection();

                // Removing any existing configurations of AppDbContext, ensuring a clean database context
                if (descriptor is not null)
                {
                    services.Remove(descriptor);
                }

                if (descriptor2 is not null)
                {
                    services.Remove(descriptor2);
                }

                // Adding a new database context based on the new connection string
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseNpgsql(_dbContainer.GetConnectionString());
                });
            });
        }

        // Closing database container 
        public new Task DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }
    }
}