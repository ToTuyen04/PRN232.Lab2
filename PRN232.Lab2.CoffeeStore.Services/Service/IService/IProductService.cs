using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using PRN232.Lab2.CoffeeStore.Services.RequestModel;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Services.Service.IService
{
    public interface IProductService
    {
        Task<ProductResponse> AddAsync(ProductRequest obj);
        Task<ProductResponse> UpdateAsync(int id, ProductRequest obj);
        Task DeleteAsync(Product obj);
        Task<ProductResponse> GetByIdAsync(int id);
        Task<Paginated<ProductResponse>> GetAllAsync(string search, int currentPage, int pageSize, string orderBy, string select);


        void Add(Product obj);
        void Update(Product obj);
        void Delete(Product obj);
        Product GetById(int id);
        IEnumerable<Product> GetAll();
    }
}
