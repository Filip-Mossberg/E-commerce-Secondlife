using E_commerce.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.DAL.IRepository
{
    public interface ICategoryRepository
    {
        public Task CreateCategory(Category category);
        public Task<Category> GetCategoryByName(string name);
        public Task<Category> GetCategoryById(int id);  
        public Task<IEnumerable<Category>> GetAllCategories();
        public Task UpdateCategory(Category category);
        public Task DeleteCategory(Category category);
    }
}
