using E_commerce.Models;
using E_commerce.Models.DTO_s.Category;
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
            var command = new CategoryCreateRequest()
            {
                Name = "Boat",
            };

            // Act 
            var result = await Sender.Send(command) as ApiResponse;

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
