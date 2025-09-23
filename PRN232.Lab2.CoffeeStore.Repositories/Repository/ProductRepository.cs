using Azure;
using Microsoft.EntityFrameworkCore;
using PRN232.Lab2.CoffeeStore.Repositories.Context;
using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using PRN232.Lab2.CoffeeStore.Repositories.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Repositories.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly CoffeeStoreDbContext _db;
        public ProductRepository(CoffeeStoreDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<(dynamic Items, int TotalCount)> GetPaginatedAsync(
            string search, 
            int currentPage, 
            int pageSize, 
            string orderBy, 
            string select
            )
        {
            var query = _db.Product.AsQueryable();

            if(string.IsNullOrEmpty(select) || select.ToLower().Contains("categoryname") || select.ToLower().Contains("category"))
            {
                query = query.Include(p => p.Category);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                query = ApplySorting(query, orderBy);
            }

            var totalCount = await query.CountAsync();
            query = query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);
            if (string.IsNullOrEmpty(select))
            {
                var products = await query.ToListAsync();
                return (products, totalCount);
            } else
            {
                var items = await ApplySelectFields(query, select);
                return (items, totalCount);
            }
        }

        private async Task<IEnumerable<object>> ApplySelectFields(IQueryable<Product> query, string select)
        {
            var fields = select.Split(',').Select(f => f.Trim().ToLower()).ToHashSet();

            var result = await query.Select(p => new
            {
                ProductId = fields.Contains("productid") || fields.Contains("id") ? (int?)p.ProductId : null,
                Name = fields.Contains("name") ? p.Name : null,
                Description = fields.Contains("description") ? p.Description : null,
                Price = fields.Contains("price") ? (decimal?)p.Price : null,
                IsActive = fields.Contains("isactive") || fields.Contains("active") ? (bool?)p.IsActive : null,
                CategoryName = fields.Contains("categoryname") || fields.Contains("category") ? p.Category.Name : null
            }).ToListAsync();

            // Convert to clean objects containing only selected fields
            return result.Select(item =>
            {
                var obj = new Dictionary<string, object>();
                
                if (item.ProductId.HasValue) obj["productId"] = item.ProductId.Value;
                if (item.Name != null) obj["name"] = item.Name;
                if (item.Description != null) obj["description"] = item.Description;
                if (item.Price.HasValue) obj["price"] = item.Price.Value;
                if (item.IsActive.HasValue) obj["isActive"] = item.IsActive.Value;
                if (item.CategoryName != null) obj["categoryName"] = item.CategoryName;
                
                return obj;
            });
        }

        private IQueryable<Product> ApplySorting(IQueryable<Product> query, string orderBy)
        {
            var sortFields = orderBy.Split(',');

            var ordering = string.Join(",", sortFields.Select(f =>
                f.StartsWith("-") ? f.Substring(1) + " desc" : f + " asc"
            ));

            return query.OrderBy(ordering); // Dynamic LINQ
        }

        public new async Task<Product> GetAsync(Expression<Func<Product, bool>> filter)
        {
            return await _db.Product.Include(p => p.Category).FirstOrDefaultAsync(filter);
        }

        public new async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _db.Product.Include(p => p.Category).ToListAsync();
        }

        public void Update(Product obj)
        {
            _db.Product.Update(obj);
        }
    }
}
