using AutoMapper;
using E_commerce.BLL.IService;
using E_commerce.DAL.IRepository;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Product;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Serilog;

namespace E_commerce.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageService _imageService;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductCreateRequest> _productCreateValidator;
        private readonly IValidator<ProductUpdateRequest> _productUpdateValidator;
        public ProductService(IProductRepository productRepository, IMapper mapper, IValidator<ProductCreateRequest> productCreateValidator
            ,IImageService imageService, IValidator<ProductUpdateRequest> productUpdateValidator)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _productCreateValidator = productCreateValidator;
            _imageService = imageService;
            _productUpdateValidator = productUpdateValidator;
        }

        public async Task<ApiResponse> CreateProduct(ProductCreateRequest productCreateRequest)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var validationResult = await _productCreateValidator.ValidateAsync(productCreateRequest);

            if (validationResult.IsValid)
            {
                var productId = await _productRepository.CreateProduct(_mapper.Map<Product>(productCreateRequest));
                var result = await _imageService.UploadMultipleImages(productCreateRequest.Images, productId);

                if(result.IsSuccess)
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
                        response.Errors.Add("Order detauls have to change to be able to update!");
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

        public async Task<ApiResponse> ProductSearch(ProductSearchModel model)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            if(model != null)
            {
                var searchResult = await _productRepository.ProductSearch(model);

                response.IsSuccess = true;
                response.StatusCode = 200;
                response.Result = _mapper.Map<IEnumerable<ProductGetRequest>>(searchResult);
                return response;
            }
            else
            {
                response.Errors.Add("Unable to fulfill search!");
                return response;
            }
        }

        public async Task<ApiResponse> GetAllByUserId(string userId)
        {
           ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            var products = await _productRepository.GetAllProductsByUserId(userId);

            if (products.Any())
            {
                response.IsSuccess = true;
                response.StatusCode = 200;
                response.Result = _mapper.Map<IEnumerable<ProductGetRequest>>(products);
                return response;
            }
            else
            {
                response.Errors.Add("You dont have any products!");
                return response;
            }
        }
    }
}
