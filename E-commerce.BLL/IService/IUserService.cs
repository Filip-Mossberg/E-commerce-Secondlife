using E_commerce.Models;
using E_commerce.Models.DTO_s.User;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce_BLL.IService
{
    public interface IUserService
    {
        public Task<ApiResponse> UserRegister(UserRegisterRequest userRegisterReq, HttpContext httpContext);
        public Task<ApiResponse> GetUserById(string id);
        public Task<ApiResponse> DeleteUserById(string id);
        public Task<ApiResponse> UpdateUserPassword(UserUpdatePasswordRequest userUpdateReq);
        public Task<ApiResponse> UserLogin(UserLoginRequest userLoginReq);
    }
}
