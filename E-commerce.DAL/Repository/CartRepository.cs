using E_commerce.Context;
using E_commerce.DAL.IRepository;
using E_commerce.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Cart> GetCartById(int id)
        {
            return await _context.Cart.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> RemoveAllFromCart(int cartId)
        {
            var cartProductList = await _context.Cart.Include(p => p.Products).FirstOrDefaultAsync(c => c.Id == cartId);

            if (cartProductList != null)
            {
                cartProductList.Products.Clear();

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveItemFromCart(int cartId, Product product)
        {
            var cartProductList = await _context.Cart.Include(cart => cart.Products).FirstOrDefaultAsync(c => c.Id == cartId);

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
