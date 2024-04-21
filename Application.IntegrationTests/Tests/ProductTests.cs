using E_commerce.BLL.IService;
using E_commerce.BLL.Service.Consumer.ProductConsumer;
using E_commerce.DAL.IRepository;
using E_commerce.DAL.Repository.Get;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Image;
using E_commerce.Models.DTO_s.Product;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text;

namespace Application.IntegrationTests.Tests
{
    // Product Controller Tests
    public class ProductTests : IClassFixture<TestClassFixture>
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageService _imageService;
        private readonly TestClassFixture _fixture;
        public ProductTests(TestClassFixture fixture)
        {
            _fixture = fixture;

            _productRepository = _fixture._scope.ServiceProvider.GetRequiredService<IProductRepository>();
            _imageService = _fixture._scope.ServiceProvider.GetRequiredService<IImageService>();
        }

        [Fact]
        [Category("POST")]
        public async Task Should_Call_The_Product_CreateMassTransit_Endpoint_With_Sucessful_Statuscode()
        {
            // Arrange
            var images = new List<ImageUploadRequest>()
            {
                 new ImageUploadRequest
                 {
                     FilePath = "C:\\Users\\Joakim\\source\\repos\\E-commerce\\Application.IntegrationTests\\Images\\Laptop.jpeg",
                     IsDisplayImage = true
                 }
            };

            var request = new ProductCreateRequest("Dell Monitor", "Up for sale is a brand - new Dell Monitor, still sealed in its original packaging, waiting to transform your computing experience. " +
                "This high-quality monitor boasts impressive features and cutting-edge technology to enhance productivity, entertainment, and everything in between.",
                1500, images, _fixture._userId, 3
            );
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _fixture._client.PostAsync("/api/Product/CreateMassTransit", content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        [Category("POST")]
        public async Task Should_Add_New_Product_To_The_Database_Through_The_ProductCreateConsumer()
        {
            // Arrange 
            var consumer = new ProductCreateConsumer(_productRepository, _fixture._mapper, _imageService);

            var images = new List<ImageUploadRequest>()
            {
                 new ImageUploadRequest
                 {
                     FilePath = "C:/Users/Joakim/source/repos/E-commerce/Application.IntegrationTests/Images/Laptop.jpeg",
                     IsDisplayImage = true
                 }
            };

            var request = new ProductCreateRequest("Dell Monitor", "Up for sale is a brand - new Dell Monitor, still sealed in its original packaging, waiting to transform your computing experience. " +
                "This high-quality monitor boasts impressive features and cutting-edge technology to enhance productivity, entertainment, and everything in between.",
                1500, images, _fixture._userId, 3
            );

            var mockConsumeContext = new Mock<ConsumeContext<ProductCreateRequest>>();
            mockConsumeContext.Setup(c => c.Message).Returns(request);

            // Act
            await consumer.Consume(mockConsumeContext.Object);

            // Assert
            var product = await _fixture._context.Product.FirstOrDefaultAsync(p => p.Title == "Dell Monitor");

            Assert.True(product != null && _fixture._context.Image.Any(p => p.ProductId == product.Id));
        }

        [Fact]
        [Category("PUT")]
        public async Task Should_Call_The_Product_UpdateMassTransit_Endpoint_With_Successful_Statuscode()
        {
            // Arrange
            var request = new ProductUpdateRequest()
            {
                Id = 2,
                Title = "Autonomous Electric Sedan",
                Description = "Experience the future of driving with our Autonomous Electric Sedan. " +
                "Equipped with cutting-edge self-driving technology and a fully electric engine, this car offers a seamless, eco-friendly ride.",
                Price = 40000
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var response = await _fixture._client.PutAsync("/api/Product/UpdateMassTransit", content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        [Category("PUT")]
        public async Task Should_Update_Product_In_The_Database_Through_The_ProductUpdateConsumer()
        {
            // Arrange
            var consumer = new ProductUpdateConsumer(_productRepository, _fixture._cache);

            var request = new ProductUpdateRequest()
            {
                Id = 2,
                Title = "Autonomous Electric Sedan",
                Description = "Experience the future of driving with our Autonomous Electric Sedan. " +
                "Equipped with cutting-edge self-driving technology and a fully electric engine, this car offers a seamless, eco-friendly ride.",
                Price = 40000
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            // Act
            var mockConsumeContext = new Mock<ConsumeContext<ProductUpdateRequest>>();
            mockConsumeContext.Setup(p => p.Message).Returns(request);

            await consumer.Consume(mockConsumeContext.Object);

            // Assert
            Assert.False(_fixture._context.Product.Any(p => p.Title == "Wi-Fi Enabled Smart Kettle"));
            Assert.True(_fixture._context.Product.Any(p => p.Title == "Autonomous Electric Sedan"));
        }

        [Fact]
        public async Task Should_Get_Single_Product_Through_The_GetSingleProductRedis_Endpoint()
        {
            // Arrange
            int id = 1;

            // Act 
            var response = await _fixture._client.GetAsync($"/api/Product/GetSingleProductRedis?productId={id}");

            ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.True(response.IsSuccessStatusCode);

            if (apiResponse.Result != null)
            {
                // First turning Result into a string, apiResponse.Result is of type object but we know what the type is so we can deserialize it.
                var product = JsonConvert.DeserializeObject<ProductGetResponse>(apiResponse.Result.ToString());
                Assert.NotNull(product);
            }
            else
            {
                Assert.False(true, "Product is null.");
            }
        }


        [Theory]
        [InlineData(null, "title", "desc", 2, 1, 4)]
        async Task Should_Get_All_Products_Through_The_GetAllRedis_Endpoint(string searchTerm, string sortColumn,
            string sortOrder, int category, int page, int pageSize)
        {
            // Act 
            var response = await _fixture._client.GetAsync($"/api/Product/GetAllRedis?searchTerm={searchTerm}&sortColumn={sortColumn}" +
                $"&sortOrder={sortOrder}&category={category}&page={page}&pageSize={pageSize}");

            ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(await response.Content.ReadAsStringAsync());


            var products = apiResponse.Result as PageList<ProductGetResponse>;

            // Assert
            Assert.True(response.IsSuccessStatusCode);

            if (apiResponse.Result != null)
            {
                var product = JsonConvert.DeserializeObject<PageList<ProductGetResponse>>(apiResponse.Result.ToString());
                Assert.True(1 <= product.items.Count);
            }
            else
            {
                Assert.False(true, "No products found.");
            }
        }
    }
}
