using E_commerce.Context;
using E_commerce.Models.DbModels;
using E_commerce_DAL.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        public UserRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task DeleteUserById(User user)
        {
            await _userManager.DeleteAsync(user);
        }

        public async Task<User> GetUserById(string id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<IdentityResult> UserRegister(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UserPasswordUpdate(User user, string currentPassword, string password)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, password);
        }
    }
}
