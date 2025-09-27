using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.RequestModel
{
    [XmlRoot("OrderRequest")]
    public class OrderRequest
    {
        [XmlElement("UserId")]
        public string UserId { get; set; }
        [XmlElement("PaymentMethod")]
        public string PaymentMethod { get; set; }
        [XmlArray("OrderItems")]
        [XmlArrayItem("OrderItem")]
        public ICollection<OrderItemRequest>? Items { get; set; }
    }
    [XmlRoot("OrderItemRequest")]
    public class OrderItemRequest
    {
        [XmlElement("ProductId")]
        public int ProductId { get; set; }
        [XmlElement("Quantity")]
        public int Quantity { get; set; }
    }
    [XmlRoot("OrderUpdateStatusRequest")]
    public class OrderUpdateStatusRequest
    {
        [XmlElement("Status")]
        public string Status { get; set; }
    }
}
