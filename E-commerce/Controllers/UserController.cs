using E_commerce.Middleware.Exceptions;
using E_commerce.Models.DTO_s.User;
using E_commerce_BLL.IService;
using E_commerce_DAL.IRepository;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace E_commerce_recycling.Controllers
{
    [Route("Api/User")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> UserRegister(UserRegisterRequest userRegisterReq)
        {
            var response = await _userService.UserRegister(userRegisterReq);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var response = await _userService.GetUserById(id);
            Log.Information("ApiResponse object => {@response}", response);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
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

        [HttpPut("Test")]
        public async Task<int> Divide(int number)
        {
            if (number == 1)
            {
                var result = number / 0;
                return result;
            }
            else if(number == 2)
            {
                throw new ArgumentNullException("Not Found");
            }
            else
            {
                throw new ("Bad Request");
            }
        }
    }
}
