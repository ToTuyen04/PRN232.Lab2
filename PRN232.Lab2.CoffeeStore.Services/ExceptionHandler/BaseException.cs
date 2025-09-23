namespace PRN232.Lab2.CoffeeStore.Services.ExceptionHandler
{
    public class BaseException : Exception
    {
        public string ErrorCode { get; set; }
        public BaseException(string message, string errorCode = null) : base(message)
        {
            ErrorCode = errorCode;
        }

        public BaseException(string message, Exception innerException, string errorCode = null) 
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
