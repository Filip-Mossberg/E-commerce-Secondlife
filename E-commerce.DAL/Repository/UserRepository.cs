using E_commerce.Context;
using E_commerce.Models.DbModels;
using E_commerce_DAL.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        public UserRepository(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task DeleteUserById(User user)
        {
            await _userManager.DeleteAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserById(string id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task UserRegister(User user, string password)
        {
            await _userManager.CreateAsync(user, password);
            await _context.SaveChangesAsync();
        }
    }
}
