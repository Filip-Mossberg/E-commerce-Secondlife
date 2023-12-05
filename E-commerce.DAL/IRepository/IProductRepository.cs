using E_commerce.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.DAL.IRepository
{
    public interface IProductRepository
    {
        public Task<IEnumerable<Product>> GetAllByCategoryId(int categoryId);
        public Task GetAllByCustomerId(int customerId);
        public Task<Product> GetProductById(int productId);
        public Task GetAllByLocation();
        public Task<IEnumerable<Product>> SearchByProductName(string productName);
        public Task<int> CreateProduct(Product product);
        public Task UpdateProduct(Product product);
        public Task DeleteProduct(Product product);
        public Task<int> UserProducts(string userId);
    }
}
