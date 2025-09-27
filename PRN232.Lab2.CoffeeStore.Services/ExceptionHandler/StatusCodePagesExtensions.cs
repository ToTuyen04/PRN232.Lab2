using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.ExceptionHandler
{
    public static class StatusCodePagesExtensions
    {
        public static IApplicationBuilder UseCustomStatusCodePages(this IApplicationBuilder app)
        {
            return app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;
                var acceptHeader = context.HttpContext.Request.Headers.Accept.ToString();

                if (response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    var errorResponse = ErrorResponse.Create(
                        "You do not have permission to access this function.",
                        "FORBIDDEN"
                    );

                    if (acceptHeader.Contains("application/xml") || acceptHeader.Contains("text/xml"))
                    {
                        response.ContentType = "application/xml; charset=utf-8";
                        var serializer = new XmlSerializer(typeof(ErrorResponse));
                        using var writer = new StringWriter();
                        serializer.Serialize(writer, errorResponse);
                        await response.WriteAsync(writer.ToString());
                    }
                    else
                    {
                        response.ContentType = "application/json; charset=utf-8";
                        var json = JsonSerializer.Serialize(errorResponse);
                        await response.WriteAsync(json);
                    }
                }
            });
        }
    }
}
