using E_commerce.BLL.IService;
using E_commerce.Models;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using E_commerce.Models.DbModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Routing;

namespace E_commerce.BLL.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly UserManager<User> _userManager;
        public EmailService(EmailConfiguration emailConfig, UserManager<User> userManager)
        {
            _emailConfig = emailConfig;
            _userManager = userManager;
        }

        public async Task<ApiResponse> ConfirmEmail(string token, string email)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var user = await _userManager.FindByEmailAsync(email);

            if(user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    response.IsSuccess = true;
                    response.StatusCode = StatusCodes.Status200OK;
                    response.Result = "Verification email sent";
                    return response;
                }
                else
                {
                    return response;
                }
            }
            else
            {
                return response;
            }
        }

        public void SendEmail(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        public async Task<ApiResponse> VerifyEmail(string email)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = StatusCodes.Status400BadRequest };
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var siteUrl = "http://localhost:5173";
                var confirmationLink = $"{siteUrl}/?token={Uri.EscapeDataString(token)}&email={user.Email}";
                var message = new EmailMessage(new string[] { user.Email! }, "Confirm email link", confirmationLink!);
                SendEmail(message);

                response.IsSuccess = true;
                response.StatusCode= StatusCodes.Status200OK;
                return response;
            }

            return response;
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };

            return emailMessage;
        }

        private void Send(MimeMessage message)
        {
            using var client = new SmtpClient();

            client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
            client.AuthenticationMechanisms.Remove("X0AUTH2");
            client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

            client.Send(message);
        }
    }
}
