using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRN232.Lab2.CoffeeStore.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Category",
                columns: new[] { "CategoryId", "CreatedDate", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Strong black coffee made by forcing steam through ground coffee beans", "Espresso" },
                    { 2, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Espresso diluted with hot water", "Americano" },
                    { 3, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Espresso with steamed milk", "Latte" },
                    { 4, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Espresso with steamed milk and milk foam", "Cappuccino" },
                    { 5, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Espresso with chocolate and steamed milk", "Mocha" },
                    { 6, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Espresso marked with a small amount of milk foam", "Macchiato" },
                    { 7, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Espresso with microfoam milk", "Flat White" },
                    { 8, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Coffee brewed with cold water for long hours", "Cold Brew" },
                    { 9, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Coffee served chilled with ice", "Iced Coffee" },
                    { 10, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Espresso poured over vanilla ice cream", "Affogato" },
                    { 11, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Traditional green tea leaves brewed hot", "Green Tea" },
                    { 12, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Strong oxidized tea leaves", "Black Tea" },
                    { 13, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tea made from herbs and flowers", "Herbal Tea" },
                    { 14, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Partially oxidized tea with floral notes", "Oolong Tea" },
                    { 15, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spiced tea mixed with milk", "Chai Latte" },
                    { 16, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Green tea powder mixed with milk", "Matcha Latte" },
                    { 17, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sweet and creamy spiced iced tea", "Thai Iced Tea" },
                    { 18, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Black tea with milk, popular in bubble tea shops", "Milk Tea" },
                    { 19, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tea infused with fruits like peach or passion fruit", "Fruit Tea" },
                    { 20, new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fragrant green tea with jasmine aroma", "Jasmine Tea" }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductId", "CategoryId", "Description", "IsActive", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "A single shot of strong espresso", true, "Espresso Single", 2.50m },
                    { 2, 1, "A double shot of strong espresso", true, "Espresso Double", 3.50m },
                    { 3, 2, "Espresso diluted with hot water", true, "Americano", 3.00m },
                    { 4, 3, "Espresso with steamed milk", true, "Latte", 4.00m },
                    { 5, 4, "Espresso with steamed milk and milk foam", true, "Cappuccino", 4.00m },
                    { 6, 5, "Espresso with chocolate and steamed milk", true, "Mocha", 4.50m },
                    { 7, 6, "Espresso marked with a small amount of milk foam", true, "Macchiato", 3.00m },
                    { 8, 7, "Espresso with microfoam milk", true, "Flat White", 4.00m },
                    { 9, 8, "Coffee brewed with cold water for long hours", true, "Cold Brew", 4.50m },
                    { 10, 9, "Coffee served chilled with ice", true, "Iced Coffee", 3.50m },
                    { 11, 10, "Espresso poured over vanilla ice cream", true, "Affogato", 5.00m },
                    { 12, 11, "Traditional green tea leaves brewed hot", true, "Green Tea", 2.00m },
                    { 13, 12, "Strong oxidized tea leaves", true, "Black Tea", 2.00m },
                    { 14, 13, "Tea made from herbs and flowers", true, "Herbal Tea", 2.50m },
                    { 15, 14, "Partially oxidized tea with floral notes", true, "Oolong Tea", 2.50m },
                    { 16, 15, "Spiced tea mixed with milk", true, "Chai Latte", 3.50m },
                    { 17, 16, "Green tea powder mixed with milk", true, "Matcha Latte", 4.00m },
                    { 18, 17, "Sweet and creamy spiced iced tea", true, "Thai Iced Tea", 3.50m },
                    { 19, 18, "Black tea with milk, popular in bubble tea shops", true, "Milk Tea", 3.00m },
                    { 20, 19, "Tea infused with fruits like peach or passion fruit", true, "Fruit Tea", 3.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Category",
                keyColumn: "CategoryId",
                keyValue: 19);
        }
    }
}
