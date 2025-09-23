namespace PRN232.Lab2.CoffeeStore.Services.ExceptionHandler
{
    public class ValidationException : BaseException
    {
        public ValidationException(string message)
         : base(message, "VALIDATION_ERROR") { }
    }
}
