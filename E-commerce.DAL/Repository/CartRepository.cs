using E_commerce.Context;
using E_commerce.DAL.IRepository;
using E_commerce.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.DAL.Repository
{
    internal class CartRepository : ICartRepository
    {
        public readonly AppDbContext _context;
        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddItemToCart(Product product, Cart cart)
        {
            var cartProductList = await _context.Cart.Include(cart => cart.Products).FirstOrDefaultAsync(c => c.Id == cart.Id);
            var productCartList = await _context.Product.Include(product => product.Carts).FirstOrDefaultAsync(c => c.Id == product.Id);

            if(cartProductList != null && productCartList != null
                && !cartProductList.Products.Contains(product) && !productCartList.Carts.Contains(cart))
            {
                cartProductList.Products.Add(product);
                productCartList.Carts.Add(cart);

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task CreateCart(Cart cart)
        {
            await _context.Cart.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetCartById(string userId)
        {
            return await _context.Cart.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<List<Product>> GetCartItems(string userId)
        {
            return await _context.Cart.Where(cart => cart.UserId == userId)
                .SelectMany(c => c.Products).Include(p => p.Images).ToListAsync();
        }

        public async Task<bool> RemoveAllFromCart(string userId)
        {
            var cartProductList = await _context.Cart.Include(p => p.Products).FirstOrDefaultAsync(c => c.UserId == userId);

            if (cartProductList != null)
            {
                cartProductList.Products.Clear();

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveItemFromCart(string userId, Product product)
        {
            var cartProductList = await _context.Cart.Include(cart => cart.Products).FirstOrDefaultAsync(c => c.UserId == userId);

            if (cartProductList != null && cartProductList.Products.Contains(product))
            {
                cartProductList.Products.Remove(product);
                await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
