using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.ResponseModel
{
    [XmlRoot("Product")]
    public class ProductResponse
    {
        [XmlElement("Id")]
        public int ProductId { get; set; }
        
        [XmlElement("ProductName")]
        public string Name { get; set; }
        
        [XmlElement("ProductDescription")]
        public string Description { get; set; }
        
        [XmlElement("ProductPrice")]
        public decimal Price { get; set; }
        
        [XmlElement("Active")]
        public bool IsActive { get; set; }
        
        [XmlElement("Category")]
        public string CategoryName { get; set; }
    }
}
