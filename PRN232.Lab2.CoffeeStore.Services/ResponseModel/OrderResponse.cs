using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.ResponseModel
{
    [XmlRoot("OrderResponse")]
    public class OrderResponse
    {
        [XmlElement("OrderId")]
        public int OrderId { get; set; }
        [XmlElement("OrderDate")]
        public DateTime OrderDate { get; set; }
        [XmlElement("Status")]
        public string Status { get; set; }
        [XmlElement("UserId")]
        public string UserId { get; set; }
        [XmlElement("UserResponse")]
        public UserResponse UserResponse { get; set; }
        [XmlElement("PaymentResponse")]
        public PaymentResponse PaymentResponse { get; set; }
        [XmlArray("OrderDetail")]
        [XmlArrayItem("OrderDetailItem")]
        public ICollection<OrderDetailResponse> OrderDetailResponses { get; set; }
    }

    [XmlRoot("OrderDetailResponse")]
    public class OrderDetailResponse
    {
        [XmlElement("OrderDetailId")]
        public int OrderDetailId { get; set; }
        [XmlElement("ProductId")]
        public int ProductId { get; set; }
        [XmlElement("ProductName")]
        public string ProductName { get; set; }
        [XmlElement("Quantity")]
        public int Quantity { get; set; }
        [XmlElement("UnitPrice")]
        public decimal UnitPrice { get; set; }
    }
    [XmlRoot("PaymentResponse")]
    public class PaymentResponse
    {
        [XmlElement("PaymentId")]
        public int PaymentId { get; set; }
        [XmlElement("PaymentMethod")]
        public string PaymentMethod { get; set; }
        [XmlElement("PaymentDate")]
        public DateTime PaymentDate { get; set; }
        [XmlElement("Amount")]
        public decimal Amount { get; set; }
    }
}
