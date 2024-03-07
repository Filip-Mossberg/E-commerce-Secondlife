using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.BLL.IService
{
    public interface ICategoryService
    {
        public Task<ApiResponse> CreateCategory(CategoryCreateRequest createCategoryReq);
        public Task<ApiResponse> GetCategoryById(int id);
        public Task<ApiResponse> GetAllCategories();
        public Task<ApiResponse> UpdateCategory(Category category);
        public Task<ApiResponse> DeleteCategoryById(int id);
        public Task<ApiResponse> GetCategoryByIdRedis(int id);
        public Task<ApiResponse> GetAllCategoriesRedis();

    }
}
