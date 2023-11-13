using AutoMapper;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.User;
using E_commerce_BLL.IService;
using E_commerce_DAL.IRepository;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_BLL.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public UserService(IUserRepository userRepository, IMapper mapper, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<ApiResponse> UserRegister(UserRegisterRequest userRegisterReq)
        {
            ApiResponse response = new ApiResponse();
            var validationContext = new ValidationContext(userRegisterReq);
            var validationResults = new List<ValidationResult>();

            var userExists = await _userManager.FindByEmailAsync(userRegisterReq.Email);

            if (Validator.TryValidateObject(userRegisterReq, validationContext, validationResults)
                && userExists == null)
            {

                User user = new User()
                {
                    UserName = userRegisterReq.UserName,
                    Email = userRegisterReq.Email
                };

                await _userRepository.UserRegister(user, userRegisterReq.PasswordHash);
                response.IsSuccess = true;

                return response;
            }
            else
            {
                response.IsSuccess = false;

                if (validationResults.Any())
                {
                    foreach (var error in validationResults)
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
            ApiResponse response = new ApiResponse();
            var user = await _userRepository.GetUserById(id);

            if (user != null)
            {
                response.IsSuccess = true;
                response.Result = _mapper.Map<UserGetRequest>(user);
                return response;
            }
            else
            {
                response.IsSuccess = false;
                response.Errors.Add($"Could not get user with Id {id}");
                return response;
            }
        }

        public async Task<ApiResponse> DeleteUserById(string id)
        {
            ApiResponse response = new ApiResponse();
            var userToDelete = await _userRepository.GetUserById(id);
            if (userToDelete != null)
            {
                await _userRepository.DeleteUserById(userToDelete);
                response.IsSuccess = true;
                return response;
            }
            else
            {
                response.IsSuccess = false;
                response.Errors.Add($"Unable to delete user with id {id}");
                return response;
            }
        }

        public async Task<ApiResponse> UpdateUserPassword(UserUpdatePasswordRequest userUpdateRequest)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false };
            var validationContext = new ValidationContext(userUpdateRequest);
            var validationResults = new List<ValidationResult>();

            if (Validator.TryValidateObject(userUpdateRequest, validationContext, validationResults))
            {
                var userToUpdate = await _userManager.FindByEmailAsync(userUpdateRequest.Email);
                if(userToUpdate != null)
                {
                    var result = await _userRepository.UserPasswordUpdate(userToUpdate, userUpdateRequest.CurrentPassword, userUpdateRequest.PasswordHash);

                    if (result.Succeeded)
                    {
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
                foreach (var error in validationResults)
                {
                    response.Errors.Add(error.ToString());
                }
                return response;
            }        
        }
    }
}
