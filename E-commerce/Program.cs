using Azure.Storage.Blobs;
using E_commerce.BLL.MiddleWeare;
using E_commerce.Context;
using E_commerce.Models;
using E_commerce_BLL;
using E_commerce_DAL;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Net;

namespace E_commerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services
                .DbServicesDAL(builder.Configuration)
                .DbServicesBLL();

            builder.Services.AddTransient<ExceptionMiddleWare>();

            builder.Services.AddSingleton<BlobContainerClient>(provider =>
            {
                var connectionString = builder.Configuration.GetValue<string>("AzureBlobStorageConnectionString");
                var blobServiceClient = new BlobServiceClient(connectionString);
                return blobServiceClient.GetBlobContainerClient("images");
            });

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            builder.Host.UseSerilog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleWare>();

            app.MapControllers();

            app.Run();
        }
    }
}