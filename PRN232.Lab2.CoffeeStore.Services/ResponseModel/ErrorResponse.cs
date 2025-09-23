using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Services.ResponseModel
{
    public class ErrorResponse : ApiResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ErrorCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Dictionary<string, List<string>>? ValidationErrors { get; set; }
        private ErrorResponse() { }

        public static ErrorResponse Create(string message, string errorCode)
        {
            return new ErrorResponse
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = errorCode
            };
        }

        //Validation errors
        public static ErrorResponse Create(string message, Dictionary<string, List<string>> validationErrors, string errorCode = "VALIDATION_ERROR")
        {
            return new ErrorResponse
            {
                IsSuccess = false,
                Message = message,
                ValidationErrors = validationErrors,
                ErrorCode = errorCode
            };
        }

        //ModelState
        public static ErrorResponse CreateFromModelState(string message, ModelStateDictionary modelState)
        {
            var errors = modelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );

            return Create(message, errors);
        }
    }
}
