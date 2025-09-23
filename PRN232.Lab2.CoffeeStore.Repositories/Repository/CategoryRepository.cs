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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly CoffeeStoreDbContext _db;
        public CategoryRepository(CoffeeStoreDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category obj)
        {
            _db.Category.Update(obj);
        }
    }
}
