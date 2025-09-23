using System.ComponentModel.DataAnnotations.Schema;

namespace PRN232.Lab2.CoffeeStore.Services.ResponseModel
{
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public string CategoryName { get; set; }
    }
}
