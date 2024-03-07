using AutoMapper;
using E_commerce.Context;
using E_commerce.DAL.IRepository;
using E_commerce.DAL.Repository.Get;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.DAL.Repository
{
    internal class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ISender _sender;
        private readonly IMapper _mapper;
        public ProductRepository(AppDbContext context, ISender sender, IMapper mapper)
        {
            _context = context;
            _sender = sender;
            _mapper = mapper;
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

        public async Task<PageList<ProductGetResponse>> GetAllProducts(string? searchTerm, string? sortColumn, string? sortOrder, int? category, int page, int pageSize)
        {
            var query = new GetProductsQuery(searchTerm, sortColumn, sortOrder, category, page, pageSize);

            var products = await _sender.Send(query);

            return products;
        }

        public async Task<IEnumerable<Product>> GetAllProductsByUserId(string userId)
        {
            return await _context.Product.Where(p => p.UserId == userId).Include(i => i.Images).ToListAsync();
        }

        public async Task<Product> GetProductById(int productId)
        {
            return await _context.Product.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<ProductGetResponse> GetSingleProduct(int productId)
        {
            var product = await _context.Product
               .Include(p => p.Images)
               .FirstOrDefaultAsync(p => p.Id == productId);

            var productResponse = _mapper.Map<ProductGetResponse>(product);

            var categoryName = await _context.Category
               .Where(c => c.Id == product.CategoryId)
               .Select(c => c.Name)
               .FirstOrDefaultAsync();

            productResponse.Category = categoryName;

            return productResponse;
        }

        public async Task UpdateProduct(Product product)
        {
            _context.Product.Update(product);
            await _context.SaveChangesAsync();  
        }

        // For validating how many active products a user have
        public async Task<int> UserProducts(string userId)
        {
            return _context.Product.Where(p => p.UserId == userId).Count();
        }
    }
}
