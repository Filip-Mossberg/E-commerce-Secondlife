using AutoMapper;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.Product;
using E_commerce.Models.DTO_s.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_BLL
{
    internal class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<UserRegisterRequest, User>();
            CreateMap<User, UserGetRequest>();
            CreateMap<ProductCreateRequest, Product>();
        }
    }
}
