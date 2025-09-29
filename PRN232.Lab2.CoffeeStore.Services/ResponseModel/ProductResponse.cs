using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using PRN232.Lab2.CoffeeStore.Services.Converters;

namespace PRN232.Lab2.CoffeeStore.Services.ResponseModel
{
    [JsonConverter(typeof(SelectiveProductResponseConverter))]
    public class ProductResponse
    {
        [JsonIgnore]
        [XmlIgnore]
        public HashSet<string> SelectedFields { get; set; } = new();
        
        public int ProductId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        
        public bool IsActive { get; set; }
        
        public string CategoryName { get; set; }
    }
}
