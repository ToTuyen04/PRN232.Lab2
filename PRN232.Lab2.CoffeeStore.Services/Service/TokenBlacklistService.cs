using PRN232.Lab2.CoffeeStore.Services.Service.IService;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Services.Service
{
    public class TokenBlacklistService : ITokenBlacklistService
    {
        private readonly IDatabase _db;

        public TokenBlacklistService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        public async Task AddToBlacklistAsync(string token, TimeSpan expiry)
        {
            await _db.StringSetAsync(token, "blacklisted", expiry);
        }

        public async Task<bool> IsBlacklistedAsync(string token)
        {
            return await _db.KeyExistsAsync(token);
        }
    }
}
