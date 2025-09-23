using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Repositories.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {

        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task AddAsync(T entity);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
    }
}
