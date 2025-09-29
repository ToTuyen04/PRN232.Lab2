using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Services.Service.IService
{
    public interface ITokenBlacklistService
    {
        Task AddToBlacklistAsync(string token, TimeSpan expiry);
        Task<bool> IsBlacklistedAsync(string token);
    }
}
