using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gbH60Services.Migrations
{
    /// <inheritdoc />
    public partial class AddDB2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 17,
                columns: new[] { "BuyPrice", "ImageURL", "Manufacturer", "SellPrice", "Stock" },
                values: new object[] { 25m, "https://static.wikia.nocookie.net/batman/images/7/74/The_Dark_Knight_Returns.jpg", "Frank Miller", 30m, 5 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 17,
                columns: new[] { "BuyPrice", "Manufacturer", "SellPrice", "Stock" },
                values: new object[] { null, null, null, 0 });
        }
    }
}
