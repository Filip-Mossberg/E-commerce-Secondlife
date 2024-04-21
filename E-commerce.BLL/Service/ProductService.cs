using AutoMapper;
using E_commerce.BLL.IService;
using E_commerce.DAL.IRepository;
using E_commerce.DAL.Repository.Get;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Product;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace E_commerce.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductCreateRequest> _productCreateValidator;
        private readonly IValidator<ProductUpdateRequest> _productUpdateValidator;
        private readonly IDistributedCache _cache;
        public ProductService(IProductRepository productRepository, IMapper mapper, IValidator<ProductCreateRequest> productCreateValidator
            ,IImageService imageService, IValidator<ProductUpdateRequest> productUpdateValidator,
            IDistributedCache cache)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _productCreateValidator = productCreateValidator;
            _imageService = imageService;
            _productUpdateValidator = productUpdateValidator;
            _cache = cache;
        }

        public async Task<ApiResponse> CreateProduct(ProductCreateRequest productCreateRequest)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var validationResult = await _productCreateValidator.ValidateAsync(productCreateRequest);

            if (validationResult.IsValid)
            {
                var product = _mapper.Map<Product>(productCreateRequest);
                product.RandomOrderIdentifier = Guid.NewGuid();

                var productId = await _productRepository.CreateProduct(product);
                var result = await _imageService.UploadMultipleImages(productCreateRequest.Images, productId);

                if (result.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.StatusCode = StatusCodes.Status201Created;
                    return response;
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        response.Errors.Add(error);
                    }
                    response.StatusCode = result.StatusCode;
                    return response;
                }
            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    response.Errors.Add(error.ErrorMessage);
                }
                return response;
            }
        }

        public async Task<ApiResponse> DeleteProductById(int productId)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var productToDelete = await _productRepository.GetProductById(productId);

            if(productToDelete != null)
            {
                if (!productToDelete.IsOrdered)
                {
                    await _imageService.DeleteImages(productId);
                    await _productRepository.DeleteProduct(productToDelete);


                    response.IsSuccess = true;
                    response.StatusCode = StatusCodes.Status200OK;
                    return response;
                }
                else
                {
                    response.Errors.Add("Cannot delete product that's ordered!");
                    return response;
                }
            }
            else
            {
                response.Errors.Add($"Unable to delete product with id {productId}");
                return response;
            }
        }

        public async Task<ApiResponse> UpdateProduct(ProductUpdateRequest updateProductRequest) 
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            var productToUpdate = await _productRepository.GetProductById(updateProductRequest.Id);
            var validationResult = await _productUpdateValidator.ValidateAsync(updateProductRequest);

            if(productToUpdate != null && validationResult.IsValid)
            {
                if (!productToUpdate.IsOrdered)
                {
                    if (productToUpdate.Title != updateProductRequest.Title || productToUpdate.Description != updateProductRequest.Description
                        || productToUpdate.Price != updateProductRequest.Price)
                    {
                        productToUpdate.Title = updateProductRequest.Title;
                        productToUpdate.Description = updateProductRequest.Description;
                        productToUpdate.Price = updateProductRequest.Price;

                        await _productRepository.UpdateProduct(productToUpdate);

                        response.IsSuccess = true;
                        response.StatusCode = StatusCodes.Status200OK;
                        return response;
                    }
                    else
                    {
                        response.Errors.Add("Order details have to change to be able to update!");
                        return response;
                    }
                }
                else
                {
                    response.Errors.Add("Cannot update product that's ordered!");
                    return response;
                }
            }
            else
            {
                if (!validationResult.IsValid)
                {
                    foreach (var error in validationResult.Errors)
                    {
                        response.Errors.Add(error.ErrorMessage);
                    }
                    return response;
                }
                else
                {
                    response.Errors.Add($"Unable to find product with id {updateProductRequest.Id}");
                    return response;
                }
            }
        }

        public async Task<ApiResponse> GetAllByUserId(string userId)
        {
           ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            var products = await _productRepository.GetAllProductsByUserId(userId);

            if (products.Any())
            {
                response.IsSuccess = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Result = _mapper.Map<IEnumerable<ProductGetResponse>>(products);
                return response;
            }
            else
            {
                response.Errors.Add("You dont have any products!");
                return response;
            }
        }

        public async Task<ApiResponse> GetAllProducts(ProductGetRequest p)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            var products = await _productRepository.GetAllProducts(p.searchTerm, p.sortColumn, p.sortOrder, p.category, p.page, p.pageSize);

            response.IsSuccess = true;
            response.StatusCode = StatusCodes.Status200OK;
            response.Result = products;
            return response;
        }

        public async Task<ApiResponse> GetAllProductsRedis(string? searchTerm, string? sortColumn, string? sortOrder, int? category, int page, int pageSize)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            string? key = $"{searchTerm?.ToLower() + sortColumn?.ToLower() + sortOrder?.ToLower() + category + page + pageSize}";

            var cachedProducts = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(cachedProducts))
            {
                var products = await _productRepository.GetAllProducts(searchTerm, sortColumn, sortOrder, category, page, pageSize);

                if (products != null)
                {
                    await _cache.SetStringAsync(key, JsonSerializer.Serialize(products));

                    response.IsSuccess = true;
                    response.StatusCode = 200;
                    response.Result = products;

                    return response;
                }

                return response;
            }

            response.IsSuccess = true;
            response.StatusCode = 200;
            response.Result = JsonSerializer.Deserialize<PageList<ProductGetResponse>>(cachedProducts);

            return response;
        }

        public async Task<ApiResponse> GetSingleProduct(int productId)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            var product = await _productRepository.GetSingleProduct(productId);

            if(product != null)
            {
                response.IsSuccess = true;
                response.StatusCode = StatusCodes.Status200OK;
                response.Result = product;
                return response;
            }
            else
            {
                response.Errors.Add("You dont have any products!");
                return response;
            }
        }

        public async Task<ApiResponse> GetSingleProductRedis(int productId)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            string key = $"product-{productId}";

            string? cachedProduct = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(cachedProduct))
            {
                var product = await _productRepository.GetSingleProduct(productId);

                if(product != null)
                {
                    await _cache.SetStringAsync(key, JsonSerializer.Serialize(product));

                    response.IsSuccess = true;
                    response.StatusCode = 200;
                    response.Result = _mapper.Map<ProductGetResponse>(product);

                    return response;
                }

                return response;
            }

            response.IsSuccess = true;
            response.StatusCode = 200;
            response.Result = JsonSerializer.Deserialize<ProductGetResponse>(cachedProduct);

            return response;
        }
    }
}
