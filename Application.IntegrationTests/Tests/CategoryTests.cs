using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Application.IntegrationTests.Tests
{
    public class CategoryTests : BaseIntegrationTest
    {
        public CategoryTests(IntegrationTestFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_Get_All_Categories()
        {
            // Arrange

            // Act 
            var test = await _context.Category.ToListAsync();

            // Assert
            Assert.NotEmpty(test);
        }
    }
}
