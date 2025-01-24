using gbH60Services.DAL;
using gbH60Services.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace gbH60ServicesTest
{
    public class CartItemTest : IDisposable
    {
        private H60assignment2DbGbContext _context { get; set; }
        private ICartItemRepository _repo { get; set; }
        public CartItemTest()
        {
            var contextOptions = new DbContextOptionsBuilder<H60assignment2DbGbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new H60assignment2DbGbContext(contextOptions);
            _context.Database.EnsureDeleted();
            _context.Customers.AddRange(
                new Customer { CustomerId = 1, FirstName = "Alice", LastName = "Smith", Email = "alice.smith@test.com", PhoneNumber = "1234567890", Province = "ON", CreditCard = "1111222233334444" },
                new Customer { CustomerId = 2, FirstName = "Bob", LastName = "Brown", Email = "bob.brown@test.com", PhoneNumber = "9876543210", Province = "QC", CreditCard = "2222333344445555" },
                new Customer { CustomerId = 3, FirstName = "Charlie", LastName = "Johnson", Email = "charlie.johnson@test.com", PhoneNumber = "5555555555", Province = "MB", CreditCard = "3333444455556666" },
                new Customer { CustomerId = 4, FirstName = "David", LastName = "Lee", Email = "david.lee@test.com", PhoneNumber = "4444444444", Province = "NB", CreditCard = "4444555566667777" }
            );
            _context.ProductCategories.AddRange(
                new ProductCategory { CategoryId = 1, ProdCat = "Kid's Ride-On Cars", ImageURL = "https://example.com/car.png" },
                new ProductCategory { CategoryId = 2, ProdCat = "T-Shirts", ImageURL = "https://example.com/tshirt.png" },
                new ProductCategory { CategoryId = 3, ProdCat = "Props/Replicas", ImageURL = "https://example.com/props.png" },
                new ProductCategory { CategoryId = 4, ProdCat = "Figures", ImageURL = "https://example.com/figures.png" },
                new ProductCategory { CategoryId = 5, ProdCat = "Comics/Graphic Novels", ImageURL = "https://example.com/comics.png" }
            );
            _context.Products.AddRange(
                 new Product { ProductId = 1, ProdCatId = 1, Name = "1989 Batmobile", Description = "Classic batmobile from Tim Burton's 1989 Batman.", ImageURL = "https://example.com/batmobile.png", Manufacturer = "GBC", Stock = 10, BuyPrice = 250m, SellPrice = 300m },
                new Product { ProductId = 2, ProdCatId = 2, Name = "Bat-Symbol Shirt", Description = "Shirt with the Bat-Symbol.", ImageURL = "https://example.com/batsymbol.png", Manufacturer = "GBC", Stock = 20, BuyPrice = 10m, SellPrice = 15m },
                new Product { ProductId = 3, ProdCatId = 3, Name = "Batarang", Description = "Steel replica of a Batarang.", ImageURL = "https://example.com/batarang.png", Manufacturer = "GBC", Stock = 5, BuyPrice = 5m, SellPrice = 10m },
                new Product { ProductId = 4, ProdCatId = 4, Name = "Dark Knight Figure", Description = "High-quality figure of Frank Miller's Dark Knight.", ImageURL = "https://example.com/darkknight.png", Manufacturer = "McFarlane Toys", Stock = 15, BuyPrice = 40m, SellPrice = 45m },
                new Product { ProductId = 5, ProdCatId = 5, Name = "The Dark Knight Returns", Description = "Frank Miller's graphic novel.", ImageURL = "https://example.com/returns.png", Manufacturer = "Frank Miller", Stock = 5, BuyPrice = 25m, SellPrice = 30m }
            );
            _context.ShoppingCarts.AddRange(
                new ShoppingCart() { CartId = 1, CustomerId = 1, DateCreated = DateTime.Now },
                new ShoppingCart() { CartId = 2, CustomerId = 2, DateCreated = DateTime.Now },
                new ShoppingCart() { CartId = 3, CustomerId = 3, DateCreated = DateTime.Now },
                new ShoppingCart() { CartId = 4, CustomerId = 4, DateCreated = DateTime.Now }
            );
            _context.SaveChanges();

            _repo = new CartItemRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void Create_CartItem_Test()
        {
            CartItem cart = new CartItem()
            {
                CartItemId = 1,
                CartId = 2,
                ProductId = 2,
                Quantity = 2,
                Price = 30
            };

            _repo.Add(cart);
            Assert.NotNull(_repo.Find(1));
        }

        [Fact]
        public void Read_CartItem_Test()
        {
            CartItem cart = new CartItem()
            {
                CartItemId = 1,
                CartId = 2,
                ProductId = 3,
                Quantity = 1,
                Price = 10
            };
            CartItem cart2 = new CartItem()
            {
                CartItemId = 2,
                CartId = 3,
                ProductId = 4,
                Quantity = 2,
                Price = 90
            };
            _repo.Add(cart);
            _repo.Add(cart2);

            Assert.True(_repo.GetAll().Count() == 2);
        }

        [Fact]
        public void Update_CartItem_Test()
        {
            CartItem cart = new CartItem()
            {
                CartItemId = 2,
                CartId = 3,
                ProductId = 4,
                Quantity = 1,
                Price = 45
            };
            _repo.Add(cart);

            cart.Quantity = 2;
            cart.Price = 90;
            _repo.Update(cart);

            Assert.True(_repo.Find(2).Price == 90);
        }

        [Fact]
        public void Delete_CartItem_Test()
        {
            CartItem cart = new CartItem()
            {
                CartItemId = 2,
                CartId = 2,
                ProductId = 2,
                Quantity = 2,
                Price = 30
            };

            _repo.Add(cart);
            _repo.Remove(2);

            Assert.Null(_repo.Find(2));
        }
    }
}