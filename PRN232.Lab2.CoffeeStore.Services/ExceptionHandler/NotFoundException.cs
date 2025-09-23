namespace PRN232.Lab2.CoffeeStore.Services.ExceptionHandler
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message)
        : base(message, "NOT_FOUND") { }
    }
}
