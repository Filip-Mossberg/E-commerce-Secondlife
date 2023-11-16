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

        public async Task GetDisplayImage(int productId) // Here we will find the Display Image of a specific product, not sure we need a method in the controller for this one 
        {

        }
    }
}
