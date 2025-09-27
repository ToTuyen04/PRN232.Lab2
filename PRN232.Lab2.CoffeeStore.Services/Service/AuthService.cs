using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using PRN232.Lab2.CoffeeStore.Repositories.Repository.IRepository;
using PRN232.Lab2.CoffeeStore.Services.ExceptionHandler;
using PRN232.Lab2.CoffeeStore.Services.RequestModel;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using PRN232.Lab2.CoffeeStore.Services.Service.IService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<AuthenResponse> LoginAsync(AuthenRequest request)
        {
            var user = _unitOfWork.User.GetUserWithEmail(request.Email);
            if(user == null)
                throw new ValidationException("Email not found");
            var verify = new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if(verify==PasswordVerificationResult.Failed)
                throw new ValidationException("Password is incorrect");
            var response = new AuthenResponse
            {
                User = _mapper.Map<UserResponse>(user),
                AccessToken = GenerateToken(user, false),
                RefreshToken = GenerateToken(user, true)
            };
            return response;
        }

        public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
        {
            var user = _unitOfWork.User.GetUserWithUsername(request.UserName);
            if(user!=null)
                throw new ValidationException("Username already exists");
            var email = _unitOfWork.User.GetUserWithEmail(request.Email);
            if(email!=null)
                throw new ValidationException("Email already exists");

            var newUser = new User();
            var passwordHash = new PasswordHasher<User>()
                .HashPassword(newUser, request.Password);
            newUser.UserId = Guid.NewGuid().ToString();
            newUser.Email = request.Email;
            newUser.UserName = request.UserName;
            newUser.PasswordHash = passwordHash;
            newUser.Role = "Customer";
            newUser.IsActive = true;
            await _unitOfWork.User.AddAsync(newUser);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<UserResponse>(newUser);

        }

        public async Task<AuthenResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            if(string.IsNullOrEmpty(request.RefreshToken))
                throw new ValidationException("Refresh token required");
            var user = await _unitOfWork.User.GetAsync(u => u.UserId == request.UserId);
            if (user == null)
                throw new ValidationException("User not found");

            ClaimsPrincipal principal;
            try
            {
                principal = ValidateJwtToken(request.RefreshToken);
            }
            catch (SecurityTokenExpiredException)
            {
                throw new ValidationException("Refresh token has expired");
            }
            catch(UnauthorizedAccessException)
            {
                throw new ValidationException("Unauthorize token");
            }
            catch (Exception)
            {
                throw new ValidationException("Invalid refresh token");
            }

            var tokenType = principal.Claims.FirstOrDefault(c => c.Type == "token_type")?.Value;
            if (tokenType != "refresh")
                throw new ValidationException("Invalid token type. Refresh token required");

            var newAccessToken = GenerateToken(user, isRefresh: false);
            var newRefreshToken = GenerateToken(user, isRefresh: true);
            return new AuthenResponse
            {
                User = _mapper.Map<UserResponse>(user),
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

        }

        private ClaimsPrincipal ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token"));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration.GetValue<string>("AppSettings:Issuer"),
                ValidateAudience = true,
                ValidAudience = _configuration.GetValue<string>("AppSettings:Audience"),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }

        private string GenerateToken(User user, bool isRefresh)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("token_type", isRefresh ? "refresh" : "access")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:Token")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
                audience: _configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: isRefresh ? DateTime.UtcNow.AddHours(2) : DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
                ); 
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
            
    }
}
