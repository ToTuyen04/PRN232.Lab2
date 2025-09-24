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
        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "{0} must be between {2} and {1} characters")]
        public string Name { get; set; }
        
        [XmlElement("Description")]
        [MaxLength(100, ErrorMessage = "{0} must be less than {1} characters")]
        public string? Description { get; set; }
        
        [XmlElement("ProductPrice")]
        [Required]
        [Range(1000, 100000000, ErrorMessage = "{0} must be between {1} and {2} characters")]
        public decimal Price { get; set; }
        
        [XmlElement("Active")]
        [Required]
        public bool IsActive { get; set; }
        
        [XmlElement("CategoryId")]
        public int CategoryId { get; set; }
    }
}
