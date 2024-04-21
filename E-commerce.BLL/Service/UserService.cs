using AutoMapper;
using E_commerce.BLL.IService;
using E_commerce.Models;
using E_commerce.Models.DbModels;
using E_commerce.Models.DTO_s.User;
using E_commerce_BLL.IService;
using E_commerce_DAL.IRepository;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_commerce_BLL.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ICartService _cartService;
        private readonly IEmailService _emailService;
        private readonly IValidator<UserRegisterRequest> _registerValidator;
        private readonly IValidator<UserUpdatePasswordRequest> _passwordUpdateValidator;
        private readonly IValidator<UserLoginRequest> _loginValidator;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IMapper mapper, UserManager<User> userManager, 
            ICartService cartService, IValidator<UserRegisterRequest> registerValidator,
            IValidator<UserUpdatePasswordRequest> updatePasswordValidator, IEmailService emailService,
            IValidator<UserLoginRequest> loginValidator, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
            _cartService = cartService;
            _registerValidator = registerValidator;
            _passwordUpdateValidator = updatePasswordValidator;
            _emailService = emailService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _configuration = configuration;
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

                if (creationResult.Succeeded)
                {
                    var createdUser = await _userManager.FindByEmailAsync(user.Email);

                    // Adding role and cart to user
                    await _userManager.AddToRoleAsync(createdUser, "user");
                    await _cartService.CreateCart(createdUser);

                    // Generating email verification token
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(createdUser);
                    var siteUrl = "http://localhost:5173";
                    var confirmationLink = $"{siteUrl}/?token={Uri.EscapeDataString(token)}&email={createdUser.Email}";
                    var message = new EmailMessage(new string[] { createdUser.Email! }, "Confirm email link", confirmationLink!);
                    _emailService.SendEmail(message);

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
                    response.Errors.Add(error.ErrorMessage);
                }
                return response;
            }        
        }

        public async Task<ApiLoginResponse> UserLogin(UserLoginRequest userLoginReq)
        {
            ApiLoginResponse response = new ApiLoginResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var validatioResult = await _loginValidator.ValidateAsync(userLoginReq);

            if (validatioResult.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLoginReq.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user, userLoginReq.Password))
                {
                    // Applying claims for the JWT token
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

                    var userRoles = await _userManager.GetRolesAsync(user);

                    // Applying user roles 
                    foreach (var role in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var JwtToken = GetToken(authClaims);

                    response.IsSuccess = true;
                    response.EmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Result = new JwtTokenHandler(JwtToken, JwtToken.ValidTo);

                    return response;
                }
                else
                {
                    response.Errors.Add("Email or password is incorrect!");
                    return response;
                }
            }
            else
            {
                foreach (var item in validatioResult.Errors)
                {
                    response.Errors.Add(item.ErrorMessage);
                }

                return response;
            }
        }

        /// <summary>
        /// JWT token configuration
        /// </summary>
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(2),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public async Task<ApiResponse> GetUserByEmail(string email)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status404NotFound, Errors = new List<string> { "No User Found!" } };
            var user = await _userRepository.GetUserByEmail(email);

            if (user != null)
            {
                response.IsSuccess = true;
                response.StatusCode = 200;
                response.Result = user;
                response.Errors = new List<string> { };

                return response;
            }

            return response;
        }
    }
}
