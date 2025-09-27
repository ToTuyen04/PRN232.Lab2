using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Repositories.Entity
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int PaymentID { get; set; }
        [ForeignKey("PaymentID")]
        public Payment Payment { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
