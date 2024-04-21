using E_commerce.BLL.Service.Consumer.CategoryConsumer;
using E_commerce.DAL.IRepository;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Category;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text;

namespace Application.IntegrationTests.Tests
{
    // Category Controller Tests
    public class CategoryTests : IClassFixture<TestClassFixture>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly TestClassFixture _fixture;
        public CategoryTests(TestClassFixture fixture)
        {
            _fixture = fixture;

            _categoryRepository = _fixture._scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
        }

        [Fact]
        [Category("POST")]
        public async Task Should_Call_The_Category_CreateMassTransit_Endpoint_With_Successful_Statuscode()
        {
            // Arrange 
            var request = new CategoryCreateRequest("Garden");
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _fixture._client.PostAsync("/api/Category/CreateMassTransit", content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        /// <summary>
        /// In a real scenario, MassTransit provides the ConsumeContext to the consumer.
        /// 
        /// Here we Mock the ConsumeContext to simulate MassTransit's behavior.
        /// </summary>
        [Fact]
        [Category("POST")]
        public async Task Should_Add_New_Category_To_The_Database_Through_The_CategoryCreateConsumer()
        {
            // Arrange
            var consumer = new CategoryCreateConsumer(_categoryRepository, _fixture._mapper);

            var request = new CategoryCreateRequest("Garden");

            var mockConsumeContext = new Mock<ConsumeContext<CategoryCreateRequest>>();
            mockConsumeContext.Setup(c => c.Message).Returns(request);

            // Act
            await consumer.Consume(mockConsumeContext.Object);

            // Assert
            Assert.True(_fixture._context.Category.Any(c => c.Name == "Garden"));
        }

        [Fact]
        [Category("PUT")]
        public async Task Should_Call_The_Category_UpdateMassTransit_Endpoint_With_Successful_Statuscode()
        {
            // Arrange
            var request = new Category(3, "Lamps");
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _fixture._client.PutAsync("/api/Category/UpdateMassTransit", content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        [Category("PUT")]
        public async Task Should_Update_Category_In_The_Database_Through_The_CategoryUpdateConsumer()
        {
            // Arrange
            var consumer = new CategoryUpdateConsumer(_categoryRepository);
            var request = new Category(2, "Clothes");

            var mockConsumeContext = new Mock<ConsumeContext<Category>>();
            mockConsumeContext.Setup(c => c.Message).Returns(request);

            // Act
            await consumer.Consume(mockConsumeContext.Object);

            // Assert
            Assert.True(_fixture._context.Category.Any(c => c.Name == "Clothes"));
        }

        [Fact]
        public async Task Should_Get_All_Categories_Through_The_GetAllRedis_Endpoint()
        {
            // Act
            var response = await _fixture._client.GetAsync("/api/Category/GetAllRedis");

            ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.NotNull(apiResponse.Result);
        }

        [Fact]
        public async Task Should_Get_Single_Category_Through_The_GetByIdRedis_Endpoint()
        {
            // Arrange
            int id = 1;

            // Act
            var response = await _fixture._client.GetAsync($"/api/Category/GetByIdRedis?id={id}");

            ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.NotNull(apiResponse.Result);
        }
    }
}
