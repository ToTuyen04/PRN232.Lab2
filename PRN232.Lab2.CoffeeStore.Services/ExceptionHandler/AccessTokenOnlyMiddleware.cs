using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using PRN232.Lab2.CoffeeStore.Services.Helpers;

namespace PRN232.Lab2.CoffeeStore.Services.ExceptionHandler
{
    public class AccessTokenOnlyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializerOptions _jsonOptions;

        public AccessTokenOnlyMiddleware(RequestDelegate next, IOptions<JsonOptions> jsonOptions)
        {
            _next = next;
            _jsonOptions = jsonOptions.Value.JsonSerializerOptions;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            //var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            //if (string.IsNullOrWhiteSpace(authHeader))
            //{
            //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            //    var errorResponse = ErrorResponse.Create(
            //        "Access token is required.",
            //        "UNAUTHORIZED"
            //    );
            //    await HelperClass.WriteErrorResponseAsync(context, errorResponse, _jsonOptions);

                //var acceptHeader = context.Request.Headers.Accept.ToString();

                //if (acceptHeader.Contains("application/xml") || acceptHeader.Contains("text/xml"))
                //{
                //    context.Response.ContentType = "application/xml; charset=utf-8";
                //    var serializer = new XmlSerializer(typeof(ErrorResponse));
                //    using var writer = new StringWriter();
                //    serializer.Serialize(writer, errorResponse);
                //    await context.Response.WriteAsync(writer.ToString());
                //}
                //else
                //{
                //    context.Response.ContentType = "application/json; charset=utf-8";
                //    var json = JsonSerializer.Serialize(errorResponse, _jsonOptions);
                //    await context.Response.WriteAsync(json);
                //}
                //return;
            //}

            if (context.User.Identity?.IsAuthenticated == true)
            {
                var tokenType = context.User.Claims.FirstOrDefault(c => c.Type == "token_type")?.Value;
                if (tokenType != "access")
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                    var errorResponse = ErrorResponse.Create(
                        "Invalid token type. Access token required.",
                        "UNAUTHORIZED"
                    );

                    var acceptHeader = context.Request.Headers.Accept.ToString();
                    await HelperClass.WriteErrorResponseAsync(context, errorResponse, _jsonOptions);

                    //if (acceptHeader.Contains("application/xml") || acceptHeader.Contains("text/xml"))
                    //{
                    //    context.Response.ContentType = "application/xml; charset=utf-8";
                    //    var serializer = new XmlSerializer(typeof(ErrorResponse));
                    //    using var writer = new StringWriter();
                    //    serializer.Serialize(writer, errorResponse);
                    //    await context.Response.WriteAsync(writer.ToString());
                    //}
                    //else
                    //{
                    //    context.Response.ContentType = "application/json; charset=utf-8";
                    //    var json = JsonSerializer.Serialize(errorResponse, _jsonOptions);
                    //    await context.Response.WriteAsync(json);
                    //}
                    //return;
                }
            }
            await _next(context);
        }
    }
}
