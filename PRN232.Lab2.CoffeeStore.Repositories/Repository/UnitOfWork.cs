using Microsoft.EntityFrameworkCore;
using PRN232.Lab2.CoffeeStore.Repositories.Context;
using PRN232.Lab2.CoffeeStore.Repositories.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Repositories.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoffeeStoreDbContext _db;
        public IProductRepository Product { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IUserRepository User { get; private set; }
        public IOrderRepository Order { get; private set; }
        public IPaymentRepository Payment { get; private set; }
        public UnitOfWork(CoffeeStoreDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            User = new UserRepository(_db);
            Order = new OrderRepository(_db);
            Payment = new PaymentRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new DbUpdateException();
            }
        }
    }
}
