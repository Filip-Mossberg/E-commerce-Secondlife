﻿using E_commerce.Models;
using E_commerce.Models.DTO_s.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_BLL.IService
{
    public interface IUserService
    {
        public Task<ApiResponse> UserRegister(UserRegisterRequest userRegisterReq);
    }
}