using E_commerce.Context;
using E_commerce.DAL.IRepository;
using E_commerce.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.DAL.Repository
{
    internal class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateProduct(Product product)
        {
            await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public Task DeleteProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public Task GetAllByCategoryId(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task GetAllByCustomerId(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task GetAllByLocation()
        {
            throw new NotImplementedException();
        }

        public Task GetProductById(int productId)
        {
            throw new NotImplementedException();
        }

        public Task SearchByProductName(string productName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProduct(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
