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
    public class ForbiddenResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public ForbiddenResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(async () =>
            {
                //nếu response đã bắt đầu thì không thể ghi body
                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    var errorResponse = ErrorResponse.Create(
                        "You do not have permission to access this function.",
                        "FORBIDDEN"
                    );

                    var acceptHeader = context.Request.Headers.Accept.ToString();

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
                        var json = JsonSerializer.Serialize(errorResponse);
                        await context.Response.WriteAsync(json);
                    }
                }

            });
            await _next(context);
        }
    }
}
