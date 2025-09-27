using Microsoft.EntityFrameworkCore;
using PRN232.Lab2.CoffeeStore.Repositories.Context;
using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using PRN232.Lab2.CoffeeStore.Repositories.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Repositories.Repository
{
    internal class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly CoffeeStoreDbContext _db;
        public OrderRepository(CoffeeStoreDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Order obj)
        {
            _db.Order.Update(obj);
        }

        public new async Task<IEnumerable<Order>> GetAllAsync()
        {
            var query = _db.Order.AsQueryable();
            query = query
                .Include(o => o.Payment)
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product);
            return await query.ToListAsync();
        }

        public new async Task<Order> GetAsync(Expression<Func<Order, bool>> filter)
        {
            var query = _db.Order.AsQueryable();
            query = query.Where(filter);
            query = query
                .Include(o => o.Payment)
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product);
            return await query.FirstOrDefaultAsync();
        }
    }
}
