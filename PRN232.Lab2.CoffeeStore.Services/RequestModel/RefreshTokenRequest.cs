using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.RequestModel
{
    [XmlRoot("RefreshTokenRequest")]
    public class RefreshTokenRequest
    {
        [XmlElement("UserId")]
        public string UserId { get; set; }
        [XmlElement("RefreshToken")]
        public string RefreshToken { get; set; }
    }
}
