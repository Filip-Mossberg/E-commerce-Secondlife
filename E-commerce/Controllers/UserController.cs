﻿using E_commerce.BLL.IService;
using E_commerce.Middleware.Exceptions;
using E_commerce.Models.DTO_s.User;
using E_commerce_BLL.IService;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace E_commerce_recycling.Controllers
{
    [ApiController]
    [Route("Api/User")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> UserRegister(UserRegisterRequest userRegisterReq)
        {
            var response = await _userService.UserRegister(userRegisterReq, Url.ActionContext.HttpContext);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var response = await _userService.GetUserById(id);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> UserDelete(string id)
        {
            var response = await _userService.DeleteUserById(id);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut("Update/Password")]
        public async Task<IActionResult> UserUpdatePassword(UserUpdatePasswordRequest userUpdateRequest)
        {
            var response = await _userService.UpdateUserPassword(userUpdateRequest);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess? Ok(response) : BadRequest(response);
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var response = await _emailService.ConfirmEmail(token, email);
            return response.IsSuccess ? Ok($"{email} is now confirmed") : BadRequest(response);
        }
    }
}
