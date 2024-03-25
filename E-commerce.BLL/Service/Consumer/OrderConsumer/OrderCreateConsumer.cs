using AutoMapper;
using E_commerce.DAL.IRepository;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Order;
using MassTransit;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Serilog;

namespace E_commerce.BLL.Service.Consumer.OrderConsumer
{
    public class OrderCreateConsumer : IConsumer<PlaceOrderRequest>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly IProductRepository _productRepository;
        public OrderCreateConsumer(IOrderRepository orderRepository, IMapper mapper, IDistributedCache cache,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _cache = cache;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Handles the processing of a PlaceOrderRequest message recieved by the message broker (RabbitMQ).
        /// With the use of Redis as distributed cache handling.
        /// </summary>
        /// <param name="context">The ConsumeContext contains a PlaceOrderRequest message.</param>
        /// <returns>A Task representing the asynchronus operation.</returns>
        public async Task Consume(ConsumeContext<PlaceOrderRequest> context)
        {
            var placedOrder = await _orderRepository.PlaceOrder(_mapper.Map<Order>(context.Message));

            foreach (var item in context.Message.Products)
            {
                string key = $"product-{item.Id}";
                string? cachedProduct = await _cache.GetStringAsync(key);

                var product = new Product();

                if (string.IsNullOrEmpty(cachedProduct))
                {
                    product = await _productRepository.GetProductById(item.Id);

                    await _cache.SetStringAsync(key, JsonConvert.SerializeObject(product));
                }
                else
                {
                    product = JsonConvert.DeserializeObject<Product>(cachedProduct);
                }

                product.IsOrdered = true;
                product.OrderId = placedOrder.Id;

                placedOrder.Products.Add(product);
                await _productRepository.UpdateProduct(product);
            }
        }
    }
}
