using E_commerce.Context;
using E_commerce.DAL.IRepository;
using E_commerce.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public async Task<int> CreateProduct(Product product)
        {
            var result = await _context.Product.AddAsync(product);
            await _context.SaveChangesAsync();
            return result.Entity.Id;
        }

        public async Task DeleteProduct(Product product)
        {
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
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

        public async Task<Product> GetProductById(int productId)
        {
            return await _context.Product.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<IEnumerable<Product>> SearchByProductName(string productName)
        {
            return await _context.Product.Where(name => Regex.IsMatch(name.Title, Regex.Escape(productName), RegexOptions.IgnoreCase)).ToListAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            _context.Product.Update(product);
            await _context.SaveChangesAsync();  
        }
    }
}
