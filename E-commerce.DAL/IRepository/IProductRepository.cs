using E_commerce.Models;
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
        public Task<Product> GetProductById(int productId);
        public Task<int> CreateProduct(Product product);
        public Task UpdateProduct(Product product);
        public Task DeleteProduct(Product product);
        public Task<IEnumerable<Product>> GetAllProductsByUserId(string userId);
        public Task<int> UserProducts(string userId);
        public Task<IEnumerable<Product>> ProductSearch(ProductSearchModel model);
    }
}
