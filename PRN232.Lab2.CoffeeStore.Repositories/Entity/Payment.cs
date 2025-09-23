using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Repositories.Entity
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
