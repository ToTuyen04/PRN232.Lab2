using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.ResponseModel
{
    [XmlRoot("UserResponse")]
    public class UserResponse
    {
        [XmlElement("UserId")]
        public string UserId { get; set; }
        [XmlElement("UserName")]
        public string UserName { get; set; }
        //[XmlElement("PasswordHash")]
        //public string PasswordHash { get; set; }
        [XmlElement("Email")]
        public string Email { get; set; }
        [XmlElement("Role")]
        public string Role { get; set; }
        [XmlElement("IsActive")]
        public bool IsActive { get; set; }
    }
}
