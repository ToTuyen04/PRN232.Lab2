using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Repositories.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product obj);
        Task<(dynamic Items, int TotalCount)> GetPaginatedAsync(
            string search, 
            int currentPage, 
            int pageSize, 
            string orderBy, 
            string select);
    }
}
