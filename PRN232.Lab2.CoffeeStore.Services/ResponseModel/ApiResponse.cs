using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.ResponseModel
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}