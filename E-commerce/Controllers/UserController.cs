using E_commerce.Models.DTO_s.User;
using E_commerce_BLL.IService;
using E_commerce_DAL.IRepository;
using Microsoft.AspNetCore.Mvc;

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
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var response = await _userService.GetUserById(id);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> UserDelete(string id)
        {
            var response = await _userService.DeleteUserById(id);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut("Update/Password")]
        public async Task<IActionResult> UserUpdatePassword(UserUpdatePasswordRequest userUpdateRequest)
        {
            var response = await _userService.UpdateUserPassword(userUpdateRequest);
            return response.IsSuccess? Ok(response) : BadRequest(response);
        }
    }
}
