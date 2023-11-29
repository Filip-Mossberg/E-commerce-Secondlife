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

namespace E_commerce.BLL.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly UserManager<User>_userManager;
        public EmailService(EmailConfiguration emailConfig, UserManager<User> userManager)
        {

            _emailConfig = emailConfig;
            _userManager = userManager;

        }

        public async Task<ApiResponse> ConfirmEmail(string token, string email)
        {
            ApiResponse response = new ApiResponse() { IsSuccess = false, StatusCode = 400 };
            var user = await _userManager.FindByEmailAsync(email);
            if(user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    response.IsSuccess = true;
                    response.StatusCode = 200;
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

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

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
