using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
    
}
