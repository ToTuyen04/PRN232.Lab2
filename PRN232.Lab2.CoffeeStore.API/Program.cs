
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN232.Lab2.CoffeeStore.Repositories.Context;
using PRN232.Lab2.CoffeeStore.Repositories.Repository;
using PRN232.Lab2.CoffeeStore.Repositories.Repository.IRepository;
using PRN232.Lab2.CoffeeStore.Services;
using PRN232.Lab2.CoffeeStore.Services.Mapper;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using PRN232.Lab2.CoffeeStore.Services.Service;
using PRN232.Lab2.CoffeeStore.Services.Service.IService;

namespace PRN232.Lab2.CoffeeStore.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<CoffeeStoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("CoffeeStoreDb"));
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddAutoMapper(typeof(Mapper));

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
                });
            //đăng ký xử lý lý validate model
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errorResponse = ErrorResponse.CreateFromModelState(
                        "Validation failed",
                        context.ModelState
                    );

                    return new BadRequestObjectResult(errorResponse);
                };
            });
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
