using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace gbH60Services.Migrations
{
    /// <inheritdoc />
    public partial class AddDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "CreditCard", "Email", "FirstName", "LastName", "PhoneNumber", "Province" },
                values: new object[,]
                {
                    { 1, "1111222233334444", "john-doe@email.com", "John", "Doe", "1234567890", "ON" },
                    { 2, "1111222233334444", "jane-doe@email.com", "Jane", "Doe", "1234567890", "QC" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategory",
                columns: new[] { "CategoryID", "ImageURL", "ProdCat" },
                values: new object[,]
                {
                    { 1, "https://www.iconpacks.net/icons/2/free-car-icon-2897-thumb.png", "Kid's Ride-On Cars" },
                    { 2, "https://cdn-icons-png.flaticon.com/512/3210/3210104.png", "T-Shirts" },
                    { 3, "https://cdn-icons-png.flaticon.com/512/4905/4905617.png", "Props/Replicas" },
                    { 4, "https://cdn-icons-png.flaticon.com/512/6967/6967649.png", "Figures" },
                    { 5, "https://cdn-icons-png.flaticon.com/512/29/29302.png", "Comics/Graphic Novels" }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "ProductID", "BuyPrice", "Description", "ImageURL", "Manufacturer", "Name", "ProdCatId", "SellPrice", "Stock" },
                values: new object[,]
                {
                    { 1, 250m, "This is the classic batmobile from Tim Burton's 1989 Batman, as a kid's car.", "https://i.imgur.com/uOoAU3k.png", "GBC", "1989 Batmobile", 1, 300m, 10 },
                    { 2, 300m, "This is the classic batmobile from Batman: The Animated Series, as a kid's car.", null, "GBC", "Batman: The Animated Series Batmobile", 1, 350m, 5 },
                    { 3, 200m, "This is the batmobile from Christopher Nolan's Dark Knight Trilogy, as a kid's car.", "https://i.imgur.com/DDVv15J.png", "GBC", "The Tumbler", 1, 250m, 15 },
                    { 4, 150m, "This is the batmobile from Matt Reeves' 2022 The Batman, as a kid's car.", null, "GBC", "The Batman (2022) Batmobile", 1, 200m, 15 },
                    { 5, 10m, "This is a shirt with the Bat-Symbol on it.", "https://i.imgur.com/CdlTueC.png", "GBC", "Bat-Symbol Shirt", 2, 15m, 20 },
                    { 6, 15m, "This is a shirt with The Joker on it.", "https://i.imgur.com/KnHLQZk.png", "GBC", "Joker Shirt", 2, 20m, 20 },
                    { 7, 20m, "This is a shirt with Harley Quinn on it.", null, "Andriu", "Harley Quinn Shirt", 2, 25m, 20 },
                    { 8, 15m, "This is a shirt with Scarecrow on it.", null, "GBC", "Scarecrow Shirt", 2, 20m, 20 },
                    { 9, 5m, "This is a steel replica of a Batarang. This is made for display and should not be thrown or used as a weapon.", "https://i.imgur.com/y3g2oOE.png", "GBC", "Batarang", 3, 10m, 5 },
                    { 10, 15m, "This is an aluminum replica of Riddler's Staff. This is made for display and should not be used as a weapon.", "https://i.imgur.com/GO1qH0z.png", "GBC", "Riddler's Staff", 3, 20m, 15 },
                    { 11, 5m, "This is a modern take on Batman's Shark Repellent from the 1966 Batman movie. This is made for display and should not be used to repel sharks or any other creatures.", "https://i.imgur.com/v3fZC0J.png", "GBC", "Shark Repellent", 3, 10m, 20 },
                    { 12, 10m, "This is a steel replica of The Joker's Playing Card weapons. This is made for display and should not be thrown or used as a weapon.", "https://i.imgur.com/U9uKlWf.png", "GBC", "Joker's Playing Cards", 3, 15m, 5 },
                    { 13, 40m, "This is a high quality figure of Frank Miller's Dark Knight.", "https://www.tftoys.ca/cdn/shop/products/McFarlaneDCMultiverseDarkKnightReturnsBatman3_large.jpg?v=1634916529", "McFarlane Toys", "Frank Miller's Dark Knight Figure", 4, 45m, 15 },
                    { 14, 300m, "This is a high quality figure of The Joker from Tim Burton's 1989 Batman movie.", "https://i.pinimg.com/474x/d0/0f/fe/d00ffeb2ef5c53614261c97d59475916.jpg", "Hot Toys", "The Joker (1989 Version) DX Series Figure", 4, 310m, 2 },
                    { 15, 100m, "This is a high quality figure of Harley Quinn from the game, Batman: Arkham Knight.", "https://m.media-amazon.com/images/I/71oyx1LdP+L._AC_UF894,1000_QL80_.jpg", "Mattel", "Arkham Knight Harley Quinn Figure", 4, 105m, 0 },
                    { 16, 150m, "This is a high quality figure of Poison Ivy from the comic, Batman: Hush.", "https://i.ebayimg.com/images/g/MjgAAOSwK~pjrWuq/s-l1200.jpg", "Medicom", "Batman: Hush Poison Ivy Figure", 4, 160m, 1 },
                    { 17, null, "This is Frank Miller's infamous graphic novel, The Dark Knight Returns.", "https://static.wikia.nocookie.net/batman", null, "The Dark Knight Returns", 5, null, 0 },
                    { 18, 25m, "In a world where Joker becomes sane and sues Batman for his senseless violence against the mentally ill.", "https://static.dc.com/dc/files/default_images/BMJKWK_01_300-001_HD_5b7f187f0c3930.50204153.jpg", "Sean Murphy", "Batman: White Knight", 5, 30m, 4 },
                    { 19, 50m, "A comic that takes place during Bruce's first year as Batman. Will he be able to save Gotham?", "https://cdn2.penguin.com.au/covers/original/9781401207526.jpg", "Frank Miller and David Mazzucchelli", "Batman: Year One", 5, 55m, 7 },
                    { 20, 20m, "A comic that explores the possible origins of The Joker, as well as a thrilling modern day story with The Clown Prince of Crime.", "https://static.wikia.nocookie.net/marvel_dc/images/1/17/Batman_The_Killing_Joke.jpg", "Alan Moore", "Batman: The Killing Joke", 5, 25m, 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "ProductID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "CategoryID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "CategoryID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "CategoryID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "CategoryID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductCategory",
                keyColumn: "CategoryID",
                keyValue: 5);
        }
    }
}
