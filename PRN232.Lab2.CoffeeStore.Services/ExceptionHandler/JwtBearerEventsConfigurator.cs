using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace PRN232.Lab2.CoffeeStore.Services.ExceptionHandler
{
    public static class JwtBearerEventsConfigurator
    {
        public static void Configure(JwtBearerOptions options)
        {
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = async context =>
                {
                    if (context.Exception is Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
                    {
                        var errorResponse = ErrorResponse.Create(
                            "Access token has expired.",
                            "TOKEN_EXPIRED"
                        );

                        var acceptHeader = context.Request.Headers["Accept"].ToString();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        if (acceptHeader.Contains("application/xml") || acceptHeader.Contains("text/xml"))
                        {
                            context.Response.ContentType = "application/xml; charset=utf-8";
                            var serializer = new XmlSerializer(typeof(ErrorResponse));
                            using var writer = new StringWriter();
                            serializer.Serialize(writer, errorResponse);
                            await context.Response.WriteAsync(writer.ToString());
                        }
                        else
                        {
                            context.Response.ContentType = "application/json; charset=utf-8";
                            var jsonOptions = new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                WriteIndented = true
                            };
                            var json = JsonSerializer.Serialize(errorResponse, jsonOptions);
                            await context.Response.WriteAsync(json);
                        }
                    }
                }
            };
        }
    }
}
