using E_commerce.BLL.IService;
using E_commerce.Middleware.Exceptions;
using E_commerce.Models.DTO_s.User;
using E_commerce_BLL.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace E_commerce_recycling.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        public UserController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("Create")]
        public async Task<IActionResult> UserRegister(UserRegisterRequest userRegisterReq)
        {
            var response = await _userService.UserRegister(userRegisterReq);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        //[Authorize(Roles = "Admin, User")]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var response = await _userService.GetUserById(id);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : NotFound(response);
        }

        //[Authorize(Roles = "Admin, User")]
        [HttpDelete("Delete")]
        public async Task<IActionResult> UserDelete(string id)
        {
            var response = await _userService.DeleteUserById(id);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        //[Authorize(Roles = "User")]
        [HttpPut("Update/Password")]
        public async Task<IActionResult> UserUpdatePassword(UserUpdatePasswordRequest userUpdateRequest)
        {
            var response = await _userService.UpdateUserPassword(userUpdateRequest);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [AllowAnonymous]
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] Confirmpass req)
        {
            var response = await _emailService.ConfirmEmail(req.token, req.email);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        public record Confirmpass(string token, string email);

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> UserLogin([FromBody] UserLoginRequest userLoginReq)
        {
            var response = await _userService.UserLogin(userLoginReq);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}
