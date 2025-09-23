using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.RequestModel
{
    [XmlRoot("ProductRequest")]
    public class ProductRequest
    {
        [XmlElement("ProductName")]
        public string Name { get; set; }
        
        [XmlElement("ProductDescription")]
        public string Description { get; set; }
        
        [XmlElement("ProductPrice")]
        public decimal Price { get; set; }
        
        [XmlElement("Active")]
        public bool IsActive { get; set; }
        
        [XmlElement("CategoryId")]
        public int CategoryId { get; set; }
    }
}
