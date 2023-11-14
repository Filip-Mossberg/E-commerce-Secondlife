using E_commerce.Models.DbModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_DAL.IRepository
{
    public interface IUserRepository
    {
        public Task<IdentityResult> UserRegister(User user, string password);
        public Task<User> GetUserById(string id);
        public Task DeleteUserById(User user);
        public Task<IdentityResult> UserPasswordUpdate(User user, string currentPassword, string password);
    }
}
