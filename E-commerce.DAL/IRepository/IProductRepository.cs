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
        public Task GetAllByCategoryId(int categoryId);
        public Task GetAllByCustomerId(int customerId);
        public Task GetProductById(int productId);
        public Task GetAllByLocation();
        public Task SearchByProductName(string productName);
        public Task CreateProduct(Product product);
        public Task UpdateProduct(Product product);
        public Task DeleteProduct(int productId);
    }
}
