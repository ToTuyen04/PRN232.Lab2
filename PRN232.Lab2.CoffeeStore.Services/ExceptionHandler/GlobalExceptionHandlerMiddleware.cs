
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using System.Text.Json;
using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.ExceptionHandler
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Lỗi không mong muốn tại {Path}. Error: {Error}",
                    context.Request.Path,
                    ex.Message
                 );
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var (statusCode, response) = exception switch
            {
                ValidationException ex => (
                    StatusCodes.Status400BadRequest,
                    ErrorResponse.Create(ex.Message, "VALIDATION_ERROR")
                ),

                NotFoundException ex => (
                    StatusCodes.Status404NotFound,
                    ErrorResponse.Create(ex.Message, "NOT_FOUND")
                ),

                DbUpdateException ex => HandleDbUpdateException(ex),

                UnauthorizedAccessException ex => (
                    StatusCodes.Status401Unauthorized,
                    ErrorResponse.Create(ex.Message, "UNAUTHORIZED")
                ),

                ArgumentNullException ex => (
                    StatusCodes.Status400BadRequest,
                    ErrorResponse.Create(ex.Message, "ARGUMENT_NULL")
                ),

                InvalidOperationException ex => (
                    StatusCodes.Status400BadRequest,
                    ErrorResponse.Create(ex.Message, "INVALID_OPERATION")
                ),

                BadRequestException ex => (
                    StatusCodes.Status400BadRequest,
                    ErrorResponse.Create(ex.Message, "BAD_REQUEST")
                ),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    CreateInternalServerError(exception)
                )
            };
            context.Response.StatusCode = statusCode;

            //await context.Response.WriteAsJsonAsync(response);
            await WriteResponseAsync(context, response);
        }

        private async Task WriteResponseAsync(HttpContext context, ErrorResponse response)
        {
            var acceptHeader = context.Request.Headers.Accept.ToString();

            if (acceptHeader.Contains("application/xml") || acceptHeader.Contains("text/xml"))
            {
                context.Response.ContentType = "application/xml; charset=utf-8";
                await WriteXmlAsync(context.Response, response);
            }
            else
            {
                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsJsonAsync(response);
            }
        }
        private async Task WriteXmlAsync<T>(HttpResponse response, T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            using var writer = new StringWriter();
            serializer.Serialize(writer, obj);
            await response.WriteAsync(writer.ToString());
        }

        private static (int StatusCode, ErrorResponse Response) HandleDbUpdateException(DbUpdateException ex)
        {
            var (message, errorCode) = ex.InnerException switch
            {
                SqlException sqlEx => sqlEx.Number switch
                {
                    2 => ("Không tìm thấy cơ sở dữ liệu", "DB_NOT_FOUND"),
                    18 => ("Lỗi đăng nhập cơ sở dữ liệu", "DB_LOGIN_ERROR"),
                    547 => ("Không thể xóa dữ liệu do ràng buộc khóa ngoại", "DB_FOREIGN_KEY_ERROR"),
                    2601 or 2627 => ("Dữ liệu bị trùng lặp", "DB_DUPLICATE_ERROR"),
                    8152 => ("Dữ liệu quá dài cho trường", "DB_DATA_TOO_LONG"),
                    515 => ("Giá trị NULL không được phép", "DB_NULL_VALUE"),
                    _ => ("Lỗi thao tác với cơ sở dữ liệu", "DB_ERROR")
                },
                _ => ("Lỗi thao tác với cơ sở dữ liệu", "DB_ERROR")
            };

            return (StatusCodes.Status400BadRequest, ErrorResponse.Create(message, errorCode));
        }

        private ErrorResponse CreateInternalServerError(Exception ex)
        {
            var message = _env.IsDevelopment()
                ? $"Internal Server Error: {ex.Message}"
                : "Đã xảy ra lỗi trong quá trình xử lý";

            return ErrorResponse.Create(message, "INTERNAL_ERROR");
        }
    }
}
