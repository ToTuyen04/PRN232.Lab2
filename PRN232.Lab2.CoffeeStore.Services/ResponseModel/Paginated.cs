using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.ResponseModel
{
    [XmlRoot("PaginatedResult")]
    //Web API, SPA
    public class Paginated<T> where T : class
    {
        [XmlElement("TotalRecords")]
        public int TotalCount { get; set; }
        
        [XmlElement("PageSize")]
        public int PageSize { get; set; }
        
        [XmlElement("CurrentPage")]
        public int CurrentPage { get; set; }
        
        [XmlElement("TotalPages")]
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        
        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public IEnumerable<T>? Items { get; set; }
    }
}

//MVC, Razor Page cần Render nút phân trang
//public class Pager<T> where T : class
//{
//    public int TotalPage { get; set; }
//    public int PageIndex { get; set; }
//    public int PageSize { get; set; }
//    public int Left { get; set; }
//    public int Right { get; set; }

//    public IEnumerable<T> List { get; set; }
//}