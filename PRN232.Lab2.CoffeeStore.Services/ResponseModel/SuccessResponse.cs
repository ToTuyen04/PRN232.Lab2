using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Services.ResponseModel
{
    public class SuccessResponse<T> : ApiResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T Data { get; set; }
        private SuccessResponse() { }

        public static SuccessResponse<T> Create(T data, string message = null)
        {
            return new SuccessResponse<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };
        }
    }
}
