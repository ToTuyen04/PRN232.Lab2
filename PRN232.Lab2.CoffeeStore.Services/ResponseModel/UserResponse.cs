using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.ResponseModel
{
    public class UserResponse
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        //[XmlElement("PasswordHash")]
        //public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
}
