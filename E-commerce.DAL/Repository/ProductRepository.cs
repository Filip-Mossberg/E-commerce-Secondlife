using E_commerce.Context;
using E_commerce.DAL.IRepository;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public async Task<IEnumerable<Product>> GetAllProductsByUserId(string userId)
        {
            return await _context.Product.Where(p => p.UserId == userId).Include(i => i.Images).ToListAsync();
        }

        public async Task<Product> GetProductById(int productId)
        {
            return await _context.Product.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<IEnumerable<Product>> ProductSearch(ProductSearchModel model)
        {
            if(model.SearchTerm != string.Empty && model.CategoryId != null)
            {
                return await _context.Product.Where(p => p.CategoryId == model.CategoryId
                && Regex.IsMatch(p.Title, Regex.Escape(model.SearchTerm), RegexOptions.IgnoreCase)
                && p.IsOrdered != true 
                && p.Price > model.MinAmount
                && p.Price < model.MaxAmount).Include(i => i.Images).ToListAsync();
            }
            else if(model.SearchTerm != string.Empty)
            {
                return await _context.Product.Where(p => Regex.IsMatch(p.Title, Regex.Escape(model.SearchTerm), RegexOptions.IgnoreCase)
                && p.IsOrdered != true
                && p.Price > model.MinAmount
                && p.Price < model.MaxAmount).Include(i => i.Images).ToListAsync(); ;
            }
            else if (model.CategoryId != null)
            {
                return await _context.Product.Where(p => p.CategoryId == model.CategoryId
                && p.IsOrdered != true
                && p.Price > model.MinAmount
                && p.Price < model.MaxAmount).Include(i => i.Images).ToListAsync(); ;
            }
            else
            {
                return await _context.Product.Where(p => p.IsOrdered != true
                && p.Price > model.MinAmount
                && p.Price < model.MaxAmount).Include(i => i.Images).ToListAsync();
            }
        }

        public async Task UpdateProduct(Product product)
        {
            _context.Product.Update(product);
            await _context.SaveChangesAsync();  
        }

        // This is for validating how many active products a user have
        public async Task<int> UserProducts(string userId)
        {
            return _context.Product.Where(p => p.UserId == userId).Count();
        }
    }
}
