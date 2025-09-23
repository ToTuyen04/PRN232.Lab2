namespace PRN232.Lab2.CoffeeStore.Services.ExceptionHandler
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message, "BAD_REQUEST")
        {
        }
    }
}
