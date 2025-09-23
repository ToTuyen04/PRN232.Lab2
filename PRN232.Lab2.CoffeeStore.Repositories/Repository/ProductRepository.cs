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
                var items = await ApplySelect(query, select);
                return (items, totalCount);
            }
        }

        private async Task<IEnumerable<object>> ApplySelect(IQueryable<Product> query, string select)
        {
            var selectFields = select.Split(',').Select(f => f.Trim().ToLower()).ToHashSet();

            // Use static select instead of dynamic string for better type safety
            if (selectFields.Contains("categoryname") && selectFields.Count == 1)
            {
                // Only CategoryName requested
                return await query.Select(p => new { CategoryName = p.Category.Name }).ToListAsync();
            }
            else if (selectFields.Contains("name") && selectFields.Count == 1)
            {
                // Only Name requested
                return await query.Select(p => new { Name = p.Name }).ToListAsync();
            }
            else if (selectFields.Contains("name") && selectFields.Contains("categoryname") && selectFields.Count == 2)
            {
                // Name and CategoryName requested
                return await query.Select(p => new { 
                    Name = p.Name, 
                    CategoryName = p.Category.Name 
                }).ToListAsync();
            }
            else
            {
                // Build dynamic select for more complex cases
                var selectParts = new List<string>();

                if (selectFields.Contains("productid") || selectFields.Contains("id"))
                    selectParts.Add("ProductId");

                if (selectFields.Contains("name"))
                    selectParts.Add("Name");

                if (selectFields.Contains("description"))
                    selectParts.Add("Description");

                if (selectFields.Contains("price"))
                    selectParts.Add("Price");

                if (selectFields.Contains("isactive") || selectFields.Contains("active"))
                    selectParts.Add("IsActive");

                if (selectFields.Contains("categoryname"))
                    selectParts.Add("CategoryName = Category.Name");

                if (selectParts.Count == 0)
                {
                    // If no valid fields specified, return all
                    return await query.Select(p => new
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        IsActive = p.IsActive,
                        CategoryName = p.Category.Name
                    }).ToListAsync();
                }

                var selectExpression = "new {" + string.Join(", ", selectParts) + "}";
                
                try
                {
                    return await query.Select(selectExpression).ToDynamicListAsync();
                }
                catch
                {
                    // Fallback to static select if dynamic fails
                    return await query.Select(p => new
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        IsActive = p.IsActive,
                        CategoryName = p.Category.Name
                    }).ToListAsync();
                }
            }
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
