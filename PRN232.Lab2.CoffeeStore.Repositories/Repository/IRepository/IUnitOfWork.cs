using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Repositories.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public IProductRepository Product { get; }
        public ICategoryRepository Category { get; }


        Task SaveAsync();
        void Save();
    }
}
