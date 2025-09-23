using Microsoft.AspNetCore.Builder;

namespace PRN232.Lab2.CoffeeStore.Services.ExceptionHandler
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}
