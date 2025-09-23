using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.ResponseModel
{
    [XmlRoot("ApiResponse")]
    public class ApiResponse
    {
        [XmlElement("IsSuccess")]
        public bool IsSuccess { get; set; }
        [XmlElement("Message")]
        public string Message { get; set; }
    }
}