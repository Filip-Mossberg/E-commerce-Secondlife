using E_commerce.Context;
using E_commerce.DAL.IRepository;
using E_commerce.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.DAL.Repository
{
    internal class ImageRepository : IImageRepository
    {
        private readonly AppDbContext _context;
        public ImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task UploadImage(Image image)
        {
            await _context.Image.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Image>> GetAllImagesById(int productId)
        {
             return await _context.Image.Where(i => i.ProductId == productId).ToListAsync();
        }

        public async Task GetDisplayImage(int productId) 
        {

        }
    }
}
