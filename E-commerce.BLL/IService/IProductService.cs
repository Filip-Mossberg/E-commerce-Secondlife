using E_commerce.Models;
using E_commerce.Models.DTO_s.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.BLL.IService
{
    public interface IProductService
    {
        public Task<ApiResponse> CreateProduct(ProductCreateRequest productCreateRequest);
        public Task<ApiResponse> DeleteProductById(int productId);
        public Task<ApiResponse> UpdateProduct(ProductUpdateRequest productUpdateRequest);
        public Task<ApiResponse> SearchByProductName(string productName);
    }
}
