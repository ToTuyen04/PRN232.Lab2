using PRN232.Lab2.CoffeeStore.Repositories.Context;
using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using PRN232.Lab2.CoffeeStore.Repositories.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}
