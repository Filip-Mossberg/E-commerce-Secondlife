using AutoMapper;
using E_commerce.DAL.IRepository;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Product;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.BLL.Service
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        //public async Task<ApiResponse> CreateProduct(ProductCreateRequest productCreateRequest)
        //{
        //    ApiResponse response = new ApiResponse();
        //    var validationContext = new ValidationContext(productCreateRequest);
        //    var validationResults = new List<ValidationResult>();

        //    if (Validator.TryValidateObject(productCreateRequest, validationContext, validationResults))
        //    {

        //        var result = await _productRepository.CreateProduct();

        //        if (result.Succeeded)
        //        {
        //            response.IsSuccess = true;
        //            return response;
        //        }
        //        else
        //        {
        //            foreach (var item in result.Errors)
        //            {
        //                response.Errors.Add(item.Description);
        //            }

        //            return response;
        //        }

        //        response.Errors.Add("Email address not valid!");
        //        return response;
        //    }
        //    else
        //    {
        //        foreach (var error in validationResults)
        //        {
        //            response.Errors.Add(error.ToString());
        //        }
        //        return response;
        //    }
        //}
    }
}
