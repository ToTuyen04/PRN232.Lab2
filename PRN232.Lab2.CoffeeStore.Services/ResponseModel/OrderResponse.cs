using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.ResponseModel
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        //[XmlElement("UserId")]
        //public string UserId { get; set; }
        public UserResponse UserResponse { get; set; }
        public PaymentResponse PaymentResponse { get; set; }
        public ICollection<OrderDetailResponse> OrderDetailResponses { get; set; }
        [JsonIgnore]
        [XmlIgnore]
        public HashSet<string> SelectedFields { get; set; } = new HashSet<string>();
    }

    public class OrderDetailResponse
    {
        public int OrderDetailId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
    public class PaymentResponse
    {
        public int PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}
