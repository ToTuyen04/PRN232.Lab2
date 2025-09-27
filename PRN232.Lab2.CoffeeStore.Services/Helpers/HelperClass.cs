using Microsoft.AspNetCore.Http;
using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PRN232.Lab2.CoffeeStore.Services.Helpers
{
    public static class HelperClass
    {
        public static bool CheckDuplicatedName(string name, IEnumerable<Object> list)
        {
            if (list is IEnumerable<Product>)
            {
                IEnumerable<Product> listProduct = list.Cast<Product>().ToList();
                foreach (Product p in listProduct)
                {
                    if (p.Name == name)
                    {
                        return true;
                    }
                }
                return false;
            }

            //if (list is IEnumerable<Menu>)
            //{
            //    List<Menu> listMenu = list.Cast<Menu>().ToList();
            //    foreach (Menu m in listMenu)
            //    {
            //        if (m.Name == name)
            //        {
            //            return true;
            //        }
            //    }
            //    return false;
            //}
            return false;
        }

        public static bool ValidateMenuDates(DateTime fromDate, DateTime toDate)
        {
            return fromDate <= toDate;
        }

        public static async Task WriteErrorResponseAsync(HttpContext context, ErrorResponse errorResponse, JsonSerializerOptions jsonOptions = null)
        {
            var acceptHeader = context.Request.Headers.Accept.ToString();

            if (acceptHeader.Contains("application/xml") || acceptHeader.Contains("text/xml"))
            {
                context.Response.ContentType = "application/xml; charset=utf-8";
                var serializer = new XmlSerializer(typeof(ErrorResponse));
                using var writer = new StringWriter();
                serializer.Serialize(writer, errorResponse);
                await context.Response.WriteAsync(writer.ToString());
            }
            else
            {
                context.Response.ContentType = "application/json; charset=utf-8";
                var json = jsonOptions != null
                    ? JsonSerializer.Serialize(errorResponse, jsonOptions)
                    : JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
        }
    }
    
}
