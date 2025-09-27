using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.RequestModel
{
    [XmlRoot("RegisterRequest")]
    public class RegisterRequest
    {
        [XmlElement("UserName")]
        public string UserName { get; set; }
        [XmlElement("Password")]
        public string Password { get; set; }
        [XmlElement("Email")]
        public string Email { get; set; }
    }
}
