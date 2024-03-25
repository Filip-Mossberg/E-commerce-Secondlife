using E_commerce.DAL.IRepository;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Product;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace E_commerce.BLL.Service.Consumer.ProductConsumer
{
    public class ProductUpdateConsumer : IConsumer<ProductUpdateRequest>
    {
        private readonly IProductRepository _productRepository;
        private readonly IDistributedCache _cache;
        public ProductUpdateConsumer(IProductRepository productRepository, IDistributedCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        /// <summary>
        /// Handles the processing of a ProductUpdateRequest message recieved by the message broker (RabbitMQ).
        /// With the use of Redis as distributed cache handling.
        /// </summary>
        /// <param name="context">The ConsumeContext contains a ProductUpdateRequest message.</param>
        /// <returns>A Task representing the asynchronus operation.</returns>
        public async Task Consume(ConsumeContext<ProductUpdateRequest> context)
        {
            string key = $"product-{context.Message.Id}";
            var cachedProduct = await _cache.GetStringAsync(key);

            var productToUpdate = new Product();

            if (cachedProduct != null)
            {
                productToUpdate = JsonConvert.DeserializeObject<Product>(cachedProduct);
            }
            else
            {
                productToUpdate = await _productRepository.GetProductById(context.Message.Id);
            }

            var product = UpdateValues(productToUpdate, context);

            await _productRepository.UpdateProduct(product);
        }

        private Product UpdateValues(Product productToUpdate, ConsumeContext<ProductUpdateRequest> context)
        {
            productToUpdate.Title = context.Message.Title;
            productToUpdate.Description = context.Message.Description;
            productToUpdate.Price = context.Message.Price;

            return productToUpdate;
        }
    }
}