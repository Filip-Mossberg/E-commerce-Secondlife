using E_commerce.BLL.IService;
using E_commerce.Models.DbModels;
using E_commerce.Models;
using E_commerce.Models.DTO_s.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using E_commerce.BLL.Validation;
using E_commerce_DAL.IRepository;

namespace E_commerce.BLL.Service.ServiceTest
{
    public class CreateUserService : IRequestHandler<UserRegisterRequest, ApiResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IValidator<UserRegisterRequest> _registerValidator;
        private readonly IUserRepository _userRepository;
        private readonly ICartService _cartService;
        private readonly IEmailService _emailService;
        private readonly ICategoryService _categoryService;
        public CreateUserService(UserManager<User> userManager, IValidator<UserRegisterRequest> registerValidator, IUserRepository userRepository, ICartService cartService, IEmailService emailService, ICategoryService categoryService)
        {
            _userManager = userManager;
            _registerValidator = registerValidator;
            _userRepository = userRepository;
            _cartService = cartService;
            _emailService = emailService;
            _categoryService = categoryService;

        }
        public async Task<ApiResponse> Handle(UserRegisterRequest request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };

            var validationResult = await _registerValidator.ValidateAsync(request);
            var userExists = await _userManager.FindByEmailAsync(request.Email);

            if (validationResult.IsValid
                && userExists == null)
            {
                User user = new User()
                {
                    Email = request.Email,
                    UserName = request.Email
                };

                var creationResult = await _userRepository.UserRegister(user, request.PasswordHash);

                if (creationResult.Succeeded)
                {
                    var createdUser = await _userManager.FindByEmailAsync(user.Email);

                    // Adding role and cart to user
                    await _userManager.AddToRoleAsync(createdUser, "user");
                    await _cartService.CreateCart(createdUser);

                    // Generating email verification token
                    //var token = await _userManager.GenerateEmailConfirmationTokenAsync(createdUser);
                    //var siteUrl = "http://localhost:5173";
                    //var confirmationLink = $"{siteUrl}/?token={Uri.EscapeDataString(token)}&email={createdUser.Email}";
                    //var message = new EmailMessage(new string[] { createdUser.Email! }, "Confirm email link", confirmationLink!);
                    //_emailService.SendEmail(message);

                    response.StatusCode = StatusCodes.Status201Created;
                    response.Result = createdUser.Id;
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
                        response.Errors.Add(error.ErrorMessage);
                    }
                }
                else
                {
                    response.Errors.Add("Email already exsists!");
                }

                return response;
            }
        }
    }
}
