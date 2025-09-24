using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using PRN232.Lab2.CoffeeStore.Services.Converters;

namespace PRN232.Lab2.CoffeeStore.Services.ResponseModel
{
    [XmlRoot("Product")]
    [JsonConverter(typeof(SelectiveProductResponseConverter))]
    public class ProductResponse
    {
        [JsonIgnore]
        [XmlIgnore]
        public HashSet<string> SelectedFields { get; set; } = new();
        
        [XmlElement("Id")]
        public int ProductId { get; set; }
        
        [XmlElement("ProductName")]
        public string Name { get; set; }
        
        [XmlElement("Description")]
        public string Description { get; set; }
        
        [XmlElement("ProductPrice")]
        public decimal Price { get; set; }
        
        [XmlElement("Active")]
        public bool IsActive { get; set; }
        
        [XmlElement("Category")]
        public string CategoryName { get; set; }
    }
}
