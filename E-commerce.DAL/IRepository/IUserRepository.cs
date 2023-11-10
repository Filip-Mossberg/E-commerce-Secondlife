using E_commerce.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_DAL.IRepository
{
    public interface IUserRepository
    {
        public Task UserRegister(User user, string password);
    }
}
