
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRN232.Lab2.CoffeeStore.Repositories.Context;
using PRN232.Lab2.CoffeeStore.Repositories.Repository;
using PRN232.Lab2.CoffeeStore.Repositories.Repository.IRepository;
using PRN232.Lab2.CoffeeStore.Services;
using PRN232.Lab2.CoffeeStore.Services.Converters;
using PRN232.Lab2.CoffeeStore.Services.ExceptionHandler;
using PRN232.Lab2.CoffeeStore.Services.Mapper;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using PRN232.Lab2.CoffeeStore.Services.Service;
using PRN232.Lab2.CoffeeStore.Services.Service.IService;
using Scalar.AspNetCore;
using System.Text;

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
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

            builder.Services.AddAutoMapper(typeof(Mapper));

            builder.Services.AddControllers(options =>
            {
                //Kích hoạt CONTENT NEGOTIATION
                options.RespectBrowserAcceptHeader = true;
                options.ReturnHttpNotAcceptable = true; //RETURN 406 nếu 0 hỗ trợ định dạng trong header Accept
            })
            .AddXmlSerializerFormatters() //thêm xml formatter
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new CustomDateTimeConverter());
                options.JsonSerializerOptions.Converters.Add(new SelectiveProductResponseConverter());
                //options.JsonSerializerOptions.PropertyNamingPolicy = null; // Giữ PascalCase
                options.JsonSerializerOptions.WriteIndented = true; // Pretty print JSON
            })
            .AddXmlDataContractSerializerFormatters(); //Thêm XML DataContract formatter

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

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var config = builder.Configuration;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["AppSettings:Issuer"],
                    ValidAudience = config["AppSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["AppSettings:Token"])),
                    ClockSkew = TimeSpan.Zero
                };
                JwtBearerEventsConfigurator.Configure(options);
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy => policy.Requirements.Add(new AdminRoleRequirement()));
            });
            builder.Services.AddSingleton<IAuthorizationHandler, AdminRoleHandler>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<CustomDateTimeSchemaFilter>();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                //app.MapScalarApiReference();
            }

            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseMiddleware<AccessTokenOnlyMiddleware>();
            app.UseAuthorization();
            //app.UseMiddleware<ForbiddenResponseMiddleware>();
            //app.UseCustomStatusCodePages();

            app.MapControllers();

            app.Run();
        }
    }
}
