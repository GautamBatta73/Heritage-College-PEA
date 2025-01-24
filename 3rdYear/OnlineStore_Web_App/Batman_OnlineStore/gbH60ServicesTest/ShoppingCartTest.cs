using gbH60Services.DAL;
using gbH60Services.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace gbH60ServicesTest
{
    public class ShoppingCartTest : IDisposable
    {
        private H60assignment2DbGbContext _context { get; set; }
        private IShoppingCartRepository _repo { get; set; }
        public ShoppingCartTest()
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
            _context.SaveChanges();

            _repo = new ShoppingCartRepository(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public void Create_ShoppingCart_Test()
        {
            ShoppingCart cart = new ShoppingCart()
            {
                CartId = 1,
                CustomerId = 2,
                DateCreated = DateTime.Now
            };

            _repo.Add(cart);
            Assert.NotNull(_repo.Find(1));
        }

        [Fact]
        public void Read_ShoppingCart_Test()
        {
            ShoppingCart cart = new ShoppingCart()
            {
                CartId = 1,
                CustomerId = 2,
                DateCreated = DateTime.Now
            };
            ShoppingCart cart2 = new ShoppingCart()
            {
                CartId = 2,
                CustomerId = 3,
                DateCreated = DateTime.Now
            };
            _repo.Add(cart);
            _repo.Add(cart2);

            Assert.True(_repo.GetAll().Count() == 2);
        }

        [Fact]
        public void Update_ShoppingCart_Test()
        {
            ShoppingCart cart = new ShoppingCart()
            {
                CartId = 2,
                CustomerId = 2,
                DateCreated = DateTime.Now
            };
            _repo.Add(cart);

            cart.CustomerId = 3;
            _repo.Update(cart);

            Assert.True(_repo.Find(2).CustomerId == 3);
        }

        [Fact]
        public void Delete_ShoppingCart_Test()
        {
            ShoppingCart cart = new ShoppingCart()
            {
                CartId = 2,
                CustomerId = 2,
                DateCreated = DateTime.Now
            };

            _repo.Add(cart);
            _repo.Remove(2);

            Assert.Null(_repo.Find(2));
        }

        [Fact]
        public void AddToCart_Test()
        {
            Product product = _context.Products.Find(4);

            _repo.AddToCart(product.ProductId, 2);
            _repo.AddToCart(product.ProductId, 2);

            CartItem cartItem = _context.CartItems.FirstOrDefault();

            Assert.True(cartItem.Quantity == 2);
            Assert.True(cartItem.Price == 90);
        }

        [Fact]
        public void RemoveFromCart_Test()
        {
            Product product = _context.Products.Find(3);
            _repo.AddToCart(product.ProductId, 1);
            _repo.AddToCart(product.ProductId, 1);

            _repo.RemoveFromCart(product.ProductId, 1);
            CartItem? cartItem = _context.CartItems.FirstOrDefault();

            Assert.Null(cartItem);
        }

        [Fact]
        public void FindByCustomer_Test()
        {
            int customerId = 3;
            ShoppingCart cart = new ShoppingCart()
            {
                CartId = 1,
                CustomerId = customerId,
                DateCreated = DateTime.Now
            };
            _repo.Add(cart);

            ShoppingCart? cart2 = _repo.FindByCustomer(customerId);

            Assert.NotNull(cart2);
        }

        [Fact]
        public void GetCartItems_Test()
        {
            int customerId = 1;
            _repo.AddToCart(3, customerId);
            _repo.AddToCart(3, customerId);
            _repo.AddToCart(2, customerId);

            IEnumerable<JoinProductCartItem> items = _repo.GetCartItems(customerId);

            Assert.True(items.Count() == 2);
        }

        [Fact]
        public void RemoveSingle_Test()
        {
            Product product = _context.Products.Find(3);
            _repo.AddToCart(product.ProductId, 1);
            _repo.AddToCart(product.ProductId, 1);

            _repo.RemoveSingle(product.ProductId, 1);
            CartItem cartItem = _context.CartItems.FirstOrDefault();

            Assert.True(cartItem.Quantity == 1);
            Assert.True(cartItem.Price == 10);
        }
    }
}