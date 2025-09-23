using Microsoft.EntityFrameworkCore;
using PRN232.Lab2.CoffeeStore.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.Lab2.CoffeeStore.Repositories.Context
{
    public class CoffeeStoreDbContext : DbContext
    {

        public CoffeeStoreDbContext(DbContextOptions<CoffeeStoreDbContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }

        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Payment> Payment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Order - Payment (1-1)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId);

            // Order - OrderDetail (1-nhiều)
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);

            // Product - OrderDetail (1-nhiều)
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId);

            // Category - Product (1-nhiều)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
            //User - Order (1-nhiều)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);


            modelBuilder.Entity<Category>()
                .HasData(
                    new Category { CategoryId = 1, Name = "Espresso", Description = "Strong black coffee made by forcing steam through ground coffee beans", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 2, Name = "Americano", Description = "Espresso diluted with hot water", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 3, Name = "Latte", Description = "Espresso with steamed milk", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 4, Name = "Cappuccino", Description = "Espresso with steamed milk and milk foam", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 5, Name = "Mocha", Description = "Espresso with chocolate and steamed milk", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 6, Name = "Macchiato", Description = "Espresso marked with a small amount of milk foam", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 7, Name = "Flat White", Description = "Espresso with microfoam milk", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 8, Name = "Cold Brew", Description = "Coffee brewed with cold water for long hours", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 9, Name = "Iced Coffee", Description = "Coffee served chilled with ice", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 10, Name = "Affogato", Description = "Espresso poured over vanilla ice cream", CreatedDate = new DateTime(2025, 9, 22) },

                    new Category { CategoryId = 11, Name = "Green Tea", Description = "Traditional green tea leaves brewed hot", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 12, Name = "Black Tea", Description = "Strong oxidized tea leaves", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 13, Name = "Herbal Tea", Description = "Tea made from herbs and flowers", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 14, Name = "Oolong Tea", Description = "Partially oxidized tea with floral notes", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 15, Name = "Chai Latte", Description = "Spiced tea mixed with milk", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 16, Name = "Matcha Latte", Description = "Green tea powder mixed with milk", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 17, Name = "Thai Iced Tea", Description = "Sweet and creamy spiced iced tea", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 18, Name = "Milk Tea", Description = "Black tea with milk, popular in bubble tea shops", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 19, Name = "Fruit Tea", Description = "Tea infused with fruits like peach or passion fruit", CreatedDate = new DateTime(2025, 9, 22) },
                    new Category { CategoryId = 20, Name = "Jasmine Tea", Description = "Fragrant green tea with jasmine aroma", CreatedDate = new DateTime(2025, 9, 22) }
                );

            modelBuilder.Entity<Product>()
                .HasData(
                    new Product { ProductId = 1, Name = "Espresso Single", Description = "A single shot of strong espresso", Price = 2.50m, IsActive = true, CategoryId = 1 },
                    new Product { ProductId = 2, Name = "Espresso Double", Description = "A double shot of strong espresso", Price = 3.50m, IsActive = true, CategoryId = 1 },
                    new Product { ProductId = 3, Name = "Americano", Description = "Espresso diluted with hot water", Price = 3.00m, IsActive = true, CategoryId = 2 },
                    new Product { ProductId = 4, Name = "Latte", Description = "Espresso with steamed milk", Price = 4.00m, IsActive = true, CategoryId = 3 },
                    new Product { ProductId = 5, Name = "Cappuccino", Description = "Espresso with steamed milk and milk foam", Price = 4.00m, IsActive = true, CategoryId = 4 },
                    new Product { ProductId = 6, Name = "Mocha", Description = "Espresso with chocolate and steamed milk", Price = 4.50m, IsActive = true, CategoryId = 5 },
                    new Product { ProductId = 7, Name = "Macchiato", Description = "Espresso marked with a small amount of milk foam", Price = 3.00m, IsActive = true, CategoryId = 6 },
                    new Product { ProductId = 8, Name = "Flat White", Description = "Espresso with microfoam milk", Price = 4.00m, IsActive = true, CategoryId = 7 },
                    new Product { ProductId = 9, Name = "Cold Brew", Description = "Coffee brewed with cold water for long hours", Price = 4.50m, IsActive = true, CategoryId = 8 },
                    new Product { ProductId = 10, Name = "Iced Coffee", Description = "Coffee served chilled with ice", Price = 3.50m, IsActive = true, CategoryId = 9 },
                    new Product { ProductId = 11, Name = "Affogato", Description = "Espresso poured over vanilla ice cream", Price = 5.00m, IsActive = true, CategoryId = 10 },
                    new Product { ProductId = 12, Name = "Green Tea", Description = "Traditional green tea leaves brewed hot", Price = 2.00m, IsActive = true, CategoryId = 11 },
                    new Product { ProductId = 13, Name = "Black Tea", Description = "Strong oxidized tea leaves", Price = 2.00m, IsActive = true, CategoryId = 12 },
                    new Product { ProductId = 14, Name = "Herbal Tea", Description = "Tea made from herbs and flowers", Price = 2.50m, IsActive = true, CategoryId = 13 },
                    new Product { ProductId = 15, Name = "Oolong Tea", Description = "Partially oxidized tea with floral notes", Price = 2.50m, IsActive = true, CategoryId = 14 },
                    new Product { ProductId = 16, Name = "Chai Latte", Description = "Spiced tea mixed with milk", Price = 3.50m, IsActive = true, CategoryId = 15 },
                    new Product { ProductId = 17, Name = "Matcha Latte", Description = "Green tea powder mixed with milk", Price = 4.00m, IsActive = true, CategoryId = 16 },
                    new Product { ProductId = 18, Name = "Thai Iced Tea", Description = "Sweet and creamy spiced iced tea", Price = 3.50m, IsActive = true, CategoryId = 17 },
                    new Product { ProductId = 19, Name = "Milk Tea", Description = "Black tea with milk, popular in bubble tea shops", Price = 3.00m, IsActive = true, CategoryId = 18 },
                    new Product { ProductId = 20, Name = "Fruit Tea", Description = "Tea infused with fruits like peach or passion fruit", Price = 3.00m, IsActive = true, CategoryId = 19 }
                );

        }
    }
}
