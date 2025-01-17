﻿using E_commerce.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.BLL.IService
{
    public interface IEmailService
    {
        public void SendEmail(EmailMessage message);
        public Task<ApiResponse> ConfirmEmail(string token, string email);
        public Task<ApiResponse> VerifyEmail(string email);
    }
}
