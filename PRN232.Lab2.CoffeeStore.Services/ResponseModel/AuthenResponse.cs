using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Services.ResponseModel
{
    public class AuthenResponse
    {
        public UserResponse User { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }
}
