using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PRN232.Lab2.CoffeeStore.Services.Helpers;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using PRN232.Lab2.CoffeeStore.Services.Service.IService;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

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

        public async Task InvokeAsync(HttpContext context, ITokenBlacklistService blacklistService)
        {

            // Kiểm tra token blacklist trước khi kiểm tra token type
            //var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            //if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Bearer "))
            //{
            //    var token = authHeader.Substring("Bearer ".Length);

            //    try
            //    {
            //        // Extract JTI từ token
            //        var tokenHandler = new JwtSecurityTokenHandler();
            //        var jwt = tokenHandler.ReadJwtToken(token);
            //        var jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

            //        if (!string.IsNullOrEmpty(jti))
            //        {
            //            // Kiểm tra token có bị blacklist không
            //            if (await blacklistService.IsBlacklistedAsync(jti))
            //            {
            //                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            //                var errorResponse = ErrorResponse.Create(
            //                    "Token has been invalidated.",
            //                    "TOKEN_BLACKLISTED"
            //                );

            //                await HelperClass.WriteErrorResponseAsync(context, errorResponse, _jsonOptions);
            //                return;
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        // Log lỗi nếu cần, nhưng không block request
            //        // Để JWT middleware xử lý token không hợp lệ
            //        Console.WriteLine($"Error checking token blacklist: {ex.Message}");
            //    }
            //}

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length);

                try
                {
                    // Đọc JWT và lấy JTI
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwt = tokenHandler.ReadJwtToken(token);
                    var jti = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                    if (!string.IsNullOrEmpty(jti))
                    {
                        // Kiểm tra JTI có bị blacklist không
                        if (await blacklistService.IsBlacklistedAsync(jti))
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                            var errorResponse = ErrorResponse.Create(
                                "Token has been invalidated.",
                                "TOKEN_BLACKLISTED"
                            );

                            await HelperClass.WriteErrorResponseAsync(context, errorResponse, _jsonOptions);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log lỗi nếu cần, không block request
                    Console.WriteLine($"Error checking token blacklist: {ex.Message}");
                }
            }

            //Kiểm tra loại token truyền vào Header phải là access token
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

                }
            }
            await _next(context);
        }
    }
}
