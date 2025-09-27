using PRN232.Lab2.CoffeeStore.Repositories.Context;
using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using PRN232.Lab2.CoffeeStore.Repositories.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Repositories.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly CoffeeStoreDbContext _db;
        public UserRepository(CoffeeStoreDbContext db) : base(db)
        {
            _db = db;
        }

        public User GetUserWithUsername(string username)
        {
            var user = _db.User.FirstOrDefault(u => u.UserName == username);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public User GetUserWithEmail(string email)
        {
            var user = _db.User.FirstOrDefault(u=>u.Email==email);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public void Update(User obj)
        {
            _db.User.Update(obj);
        }
    }
}
