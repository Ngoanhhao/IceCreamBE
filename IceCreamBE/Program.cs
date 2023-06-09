﻿using IceCreamBE.Data;
using IceCreamBE.Repository;
using IceCreamBE.Repository.Irepository;
using IceCreamBE.Repository.RepositoryTest;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace IceCreamBE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddDbContext<IceCreamDbcontext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IRepositoryRoles, RepositoryRoles>();
            builder.Services.AddScoped<IRepositoryAccounts, RepositoryAccounts>();
            builder.Services.AddScoped<IRepositoryAccountDetail, RepositoryAccountDetail>();
            builder.Services.AddScoped<IRepositoryFeedback, RepositoryFeedback>();
            builder.Services.AddScoped<IRepositoryStorage, RepositoryStorage>();
            builder.Services.AddScoped<IRepositoryProduct, RepositoryProducts>();
            builder.Services.AddScoped<IRepositoryBrand, RepositoryBrand>(); // test
            builder.Services.AddScoped<IRepositoryVourcher, RepositoryVoucher>();
            builder.Services.AddScoped<IRepositoryRecipe, RepositoryRecipe>();
            builder.Services.AddScoped<IRepositoryBill, RepositoryBill>();
            builder.Services.AddScoped<IRepositoryBillDetail, RepositoryBillDetail>();
            builder.Services.AddScoped<IRepositoryFileService, RepositoryFileService>();
            builder.Services.AddScoped<IMailHandle, MailHandle>();
            builder.Services.AddScoped<IHandleResponseCode, HandleResponseCode>();
            builder.Services.AddScoped<IToken, Token>();
            builder.Services.AddScoped<IRepositoryRefreshtoken, RepositoryRefreshToken>();


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //tự cấp token
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true, //kiểm tra hạn token
                    ClockSkew = TimeSpan.Zero,
                    //ký vào token
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:key"])),
                    //ClockSkew = TimeSpan.Zero
                };
            });


            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(setup =>
            {
                setup.AddDefaultPolicy(e =>
                {
                    e.AllowAnyMethod();
                    e.AllowAnyHeader();
                    e.SetIsOriginAllowed(origin => true);
                    //e.WithOrigins("https://localhost:8081/");
                    e.AllowCredentials();
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
            }
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseCors();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}