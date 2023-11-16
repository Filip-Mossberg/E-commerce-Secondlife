using AutoMapper;
using E_commerce.BLL.IService;
using E_commerce.DAL.IRepository;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.User;
using E_commerce_BLL.IService;
using E_commerce_DAL.IRepository;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_BLL.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ICartService _cartService;
        private readonly IValidator<UserRegisterRequest> _registerValidator;
        private readonly IValidator<UserUpdatePasswordRequest> _passwordUpdateValidator;
        public UserService(IUserRepository userRepository, IMapper mapper, UserManager<User> userManager, 
            ICartService cartService, IValidator<UserRegisterRequest> registerValidator,
            IValidator<UserUpdatePasswordRequest> updatePasswordValidator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _cartService = cartService;
            _registerValidator = registerValidator;
            _passwordUpdateValidator = updatePasswordValidator;
        }
        public async Task<ApiResponse> UserRegister(UserRegisterRequest userRegisterReq)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            var validationResult = await _registerValidator.ValidateAsync(userRegisterReq);
            var userExists = await _userManager.FindByEmailAsync(userRegisterReq.Email);

            if (validationResult.IsValid
                && userExists == null)
            {
                User user = new User()
                {
                    Email = userRegisterReq.Email,
                    UserName = userRegisterReq.Email
                };

                var creationResult = await _userRepository.UserRegister(user, userRegisterReq.PasswordHash);

                if(creationResult.Succeeded)
                {
                    var createdUser = await _userManager.FindByEmailAsync(user.Email);
                    await _cartService.CreateCart(createdUser);

                    response.StatusCode = StatusCodes.Status201Created;
                    response.IsSuccess = true;
                    return response;
                }
                else
                {
                    foreach (var item in creationResult.Errors)
                    {
                        response.Errors.Add(item.Description);
                    }

                    return response;
                }
            }
            else
            {
                if (validationResult.Errors.Any())
                {
                    foreach (var error in validationResult.Errors)
                    {
                        response.Errors.Add(error.ToString());
                    }
                }
                else
                {
                    response.Errors.Add("Email already exsists!");
                }

                return response;
            }
        }

        public async Task<ApiResponse> GetUserById(string id)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status404NotFound };
            var user = await _userRepository.GetUserById(id);

            if (user != null)
            {
                response.StatusCode = StatusCodes.Status200OK;
                response.IsSuccess = true;
                response.Result = _mapper.Map<UserGetRequest>(user);
                return response;
            }
            else
            {
                response.Errors.Add($"Could not get user with Id {id}");
                return response;
            }
        }

        public async Task<ApiResponse> DeleteUserById(string id)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var userToDelete = await _userRepository.GetUserById(id);

            if (userToDelete != null)
            {
                await _userRepository.DeleteUserById(userToDelete);

                response.StatusCode = StatusCodes.Status200OK;
                response.IsSuccess = true;
                return response;
            }
            else
            {
                response.Errors.Add($"Unable to delete user with id {id}");
                return response;
            }
        }

        public async Task<ApiResponse> UpdateUserPassword(UserUpdatePasswordRequest userUpdateRequest)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var validationResult = await _passwordUpdateValidator.ValidateAsync(userUpdateRequest);

            if (validationResult.IsValid)
            {
                var userToUpdate = await _userManager.FindByEmailAsync(userUpdateRequest.Email);
                if(userToUpdate != null)
                {
                    var result = await _userRepository.UserPasswordUpdate(userToUpdate, userUpdateRequest.CurrentPassword, userUpdateRequest.PasswordHash);

                    if (result.Succeeded)
                    {
                        response.StatusCode = StatusCodes.Status200OK;
                        response.IsSuccess = true;
                        return response;
                    }
                    else
                    {
                        foreach (var item in result.Errors)
                        {
                            response.Errors.Add(item.Description);
                        }

                        return response;
                    }
                }

                response.Errors.Add("Email address not valid!");
                return response;
            }
            else
            {
                foreach (var error in validationResult.Errors)
                {
                    response.Errors.Add(error.ToString());
                }
                return response;
            }        
        }
    }
}
