using IceCreamBE.Data;
using IceCreamBE.Repository;
using IceCreamBE.Repository.Irepository;
using IceCreamBE.Repository.RepositoryTest;
using Microsoft.EntityFrameworkCore;

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
            builder.Services.AddScoped<IRepositoryBrand, RepositoryBrandTest>(); // test
            builder.Services.AddScoped<IRepositoryVourcher, RepositoryVoucher>();
            builder.Services.AddScoped<IRepositoryRecipe, RepositoryRecipe>();

            builder.Services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}