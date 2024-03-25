using AutoMapper;
using E_commerce.BLL.IService;
using E_commerce.DAL.IRepository;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Product;
using MassTransit;

namespace E_commerce.BLL.Service.Consumer.ProductConsumer
{
    public class ProductCreateConsumer : IConsumer<ProductCreateRequest>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public ProductCreateConsumer(IProductRepository productRepository, IMapper mapper, 
            IImageService imageService)
        {
            _productRepository = productRepository;
            _mapper = mapper; 
            _imageService = imageService;
        }

        /// <summary>
        /// Handles the processing of a ProductCreateRequest message recieved by the message broker (RabbitMQ).
        /// </summary>
        /// <param name="context">The ConsumeContext contains a ProductCreateRequest message.</param>
        /// <returns>A Task representing the asynchronus operation.</returns>
        public async Task Consume(ConsumeContext<ProductCreateRequest> context)
        {
            var product = _mapper.Map<Product>(context.Message);
            product.RandomOrderIdentifier = Guid.NewGuid();

            var productId = await _productRepository.CreateProduct(product);
            await _imageService.UploadMultipleImages(context.Message.Images, productId);
        }
    }
}
