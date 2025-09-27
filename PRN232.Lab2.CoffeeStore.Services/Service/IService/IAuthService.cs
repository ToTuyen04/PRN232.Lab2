using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using PRN232.Lab2.CoffeeStore.Services.RequestModel;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Services.Service.IService
{
    public interface IAuthService
    {
        Task<UserResponse?> RegisterAsync(RegisterRequest request);
        Task<AuthenResponse> LoginAsync(AuthenRequest request);
        Task<AuthenResponse> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
