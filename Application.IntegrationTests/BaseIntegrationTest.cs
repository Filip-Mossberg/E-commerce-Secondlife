using E_commerce.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests
{
    /// <summary>
    /// BaseIntegrationTest serves as a foundation for integration tests, providing access to essential
    /// services utilized across multiple test cases. 
    /// </summary>
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestFactory>
    {
        protected readonly AppDbContext _context;
        protected BaseIntegrationTest(IntegrationTestFactory factory)
        {
            _context = factory._serviceProvider.GetRequiredService<AppDbContext>();
        }
    }
}
