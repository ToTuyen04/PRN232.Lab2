using Microsoft.EntityFrameworkCore;
using PRN232.Lab2.CoffeeStore.Repositories.Context;
using PRN232.Lab2.CoffeeStore.Repositories.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Repositories.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CoffeeStoreDbContext _db;
        private readonly DbSet<T> dbSet;
        public Repository(CoffeeStoreDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            var query = dbSet.AsQueryable();
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            var query = dbSet.AsQueryable();
            query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
