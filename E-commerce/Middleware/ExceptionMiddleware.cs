using E_commerce.Middleware.Exceptions;
using E_commerce.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.BLL.MiddleWeare
{
    public class ExceptionMiddleWare : IMiddleware
    {
        // We can take an instance of our ILogger here if we want to include that
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                await HandleException(context, exception);
            }
        }

        public static Task HandleException(HttpContext context, Exception exception)
        {
            int statusCode = StatusCodes.Status500InternalServerError;
            switch (exception)
            {
                case NotFoundException _:
                    statusCode = StatusCodes.Status404NotFound;
                    break;
                case BadRequestException _:
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
            }
            ApiResponse response = new ApiResponse()
            {
                StatusCode = statusCode,
                Errors = new List<string>() { exception.Message.ToString(), "Something went wrong." },
                IsSuccess = false,
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
