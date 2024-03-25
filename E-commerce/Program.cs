using Azure.Storage.Blobs;
using E_commerce.BLL.MiddleWeare;
using E_commerce.DAL.Context;
using E_commerce_BLL;
using E_commerce_DAL;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace E_commerce
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services
                .DbServicesDAL(builder.Configuration)
                .DbServicesBLL(builder.Configuration);

            builder.Services.AddTransient<ExceptionMiddleWare>();

            builder.Services.AddSingleton<BlobContainerClient>(provider =>
            {
                var connectionString = builder.Configuration.GetValue<string>("AzureBlobStorageConnectionString");
                var blobServiceClient = new BlobServiceClient(connectionString);
                return blobServiceClient.GetBlobContainerClient("images");
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CORSPolicy", builder =>
                {
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.WithOrigins("http://localhost:5173");
                });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer( options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
                };
            });

            builder.Services.AddMassTransit(x =>
            {
                var rabbitMqConnectionString = builder.Configuration.GetConnectionString("RabbitMQ");
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(rabbitMqConnectionString);
                });
            });

            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            builder.Host.UseSerilog();

            var app = builder.Build();

            app.ApplyMigrations(); 

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CORSPolicy");

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleWare>();

            app.MapControllers();

            app.Run();
        }
    }
}

// This is for the WebApplicationFactory<Program> in IntegrationTestFactory to use Program.cs of the E-commerce api project
public partial class Program { }