using AutoMapper;
using E_commerce.Context;
using E_commerce.Models.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests
{
    /// <summary>
    /// Provides a fixture for shared resources across test classes that implement IClassFixture<TestClassFixture>.
    /// This class initializes necessary components and resets the database before each test class starts executing
    /// to maintain test isolation. 
    /// </summary>
    public class TestClassFixture : IAsyncLifetime
    {
        public IServiceScope _scope { get; private set; }
        public AppDbContext _context { get; private set; }
        public HttpClient _client { get; private set; }
        private UserManager<User> _userManager { get; set; }
        public IMapper _mapper { get; private set; }
        public IntegrationTestFactory factory { get; private set; }
        public IDistributedCache _cache { get; private set; }
        public string _userId { get; private set; }
        private string _userId2;
        public TestClassFixture()
        {
      
        }
        public async Task DisposeAsync()
        {

        }

        // Configures shared services and reinitializes the database.
        public async Task InitializeAsync()
        {
            var factory = await IntegrationTestFactory.Instance();

            _scope = factory.Services.CreateScope();
            _context = _scope.ServiceProvider.GetRequiredService<AppDbContext>();
            _userManager = _scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            _mapper = _scope.ServiceProvider.GetRequiredService<IMapper>();
            _cache = _scope.ServiceProvider.GetRequiredService<IDistributedCache>();
            _client = factory.CreateClient();

            await _context.Database.EnsureDeletedAsync();

            await _context.Database.EnsureCreatedAsync();

            await SeedData();
        }

        private async Task SeedData()
        {
            await CreateUserAsync("hereiam5685@gmail.com", "MyTestPassword#123");

            var user = await _userManager.FindByEmailAsync("hereiam5685@gmail.com");
            _userId = user.Id;

            await CreateUserAsync("filip.mossberg@gmail.com", "MyTestPassword#123");

            var user2 = await _userManager.FindByEmailAsync("filip.mossberg@gmail.com");
            _userId2 = user2.Id;

            await _context.SaveChangesAsync();

            await CreateProductAsync("Vintage Green Uranium Glass Lamp", "This exquisite vintage lamp is crafted from green uranium glass, emitting a subtle glow under UV light. " +
                "Perfect for collectors and enthusiasts of rare vintage decor. Adds a unique, radiant touch to any room.",
                2500, _userId2, 2, false);

            await _context.SaveChangesAsync();

            await CreateProductAsync("Wi-Fi Enabled Smart Kettle", "Modernize your kitchen with our Smart Kettle." +
                " Control it remotely via Wi-Fi to boil water from anywhere, adding efficiency and convenience to your daily routine.",
                100, _userId2, 2, false);

            await _context.SaveChangesAsync();
        }

        private async Task<IdentityResult> CreateUserAsync(string email, string password)
        {
            var user = new User
            {
                UserName = email,
                Email = email
            };

            return await _userManager.CreateAsync(user, password);
        }

        private async Task CreateProductAsync(string Title, string Description, int Price, string UserId, int CategoryId, bool IsOrdered)
        {
            var product = new Product
            {
                Title = Title,
                Description = Description,
                Price = Price,
                UserId = UserId,
                CategoryId = CategoryId,
                DateListed = DateTime.UtcNow,
                IsOrdered = IsOrdered,
                RandomOrderIdentifier = Guid.NewGuid()
            };

            await _context.Product.AddAsync(product);

            await _context.SaveChangesAsync();

            var createdProduct = await _context.Product.FirstOrDefaultAsync(p => p.Title == Title);

            var image = new Image
            {
                Id = Guid.NewGuid().ToString(),
                IsDisplayImage = true,
                ProductId = createdProduct.Id,
                Url = "https://examplestorageaccount.blob.core.windows.net/images/sample-image.jpg"
            };

            await _context.Image.AddAsync(image);
        }
    }
}
