using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.RequestModel
{
    [XmlRoot("LogoutRequest")]
    public class LogoutRequest
    {
        [XmlElement("AccessToken")]
        public string AccessToken { get; set; }
        [XmlElement("RefreshToken")]
        public string RefreshToken { get; set; }
    }
}
