using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.ResponseModel
{
    public class ErrorResponse : ApiResponse
    {
        public string ErrorCode { get; set; }

        public List<ValidationError> ValidationErrors { get; set; }

        private ErrorResponse() { }

        public static ErrorResponse Create(string message, string errorCode, List<ValidationError> validationErrors = null)
        {
            return new ErrorResponse
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = errorCode,
                ValidationErrors = validationErrors ?? new List<ValidationError>()
            };
        }

        public static ErrorResponse CreateFromModelState(string message, ModelStateDictionary modelState)
        {
            var validationErrors = new List<ValidationError>();

            foreach (var kvp in modelState)
            {
                var key = kvp.Key;
                var errors = kvp.Value.Errors;

                foreach (var error in errors)
                {
                    validationErrors.Add(new ValidationError
                    {
                        Field = key,
                        Message = error.ErrorMessage
                    });
                }
            }

            return Create(message, "VALIDATION_ERROR", validationErrors);
        }
    }

    public class ValidationError
    {
        public string Field { get; set; }

        public string Message { get; set; }
    }
}
