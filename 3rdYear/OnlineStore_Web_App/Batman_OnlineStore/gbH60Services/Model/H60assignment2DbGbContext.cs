using Microsoft.EntityFrameworkCore;

namespace gbH60Services.Model;

public partial class H60assignment2DbGbContext : DbContext
{
    public H60assignment2DbGbContext()
    {
    }

    public H60assignment2DbGbContext(DbContextOptions<H60assignment2DbGbContext> options)
        : base(options)
    {
        //Database.EnsureCreated();
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId);
            entity.ToTable("Product");

            entity.HasIndex(e => e.ProdCatId, "IX_Product_ProdCatId");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.BuyPrice).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(80)
                .IsUnicode(false);
            entity.Property(e => e.SellPrice).HasColumnType("numeric(8, 2)");

            entity.HasOne(d => d.ProdCat).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProdCatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_ProductCategory");

            entity.Property(e => e.ImageURL)
            .HasMaxLength(2083)
            .IsUnicode(false);

            entity.HasData(
                new Product() { ProductId = 1, ProdCatId = 1, Name = "1989 Batmobile", Description = "This is the classic batmobile from Tim Burton's 1989 Batman, as a kid's car.", ImageURL = "https://i.imgur.com/uOoAU3k.png", Manufacturer = "GBC", Stock = 10, BuyPrice = 250m, SellPrice = 300m },
                new Product() { ProductId = 2, ProdCatId = 1, Name = "Batman: The Animated Series Batmobile", Description = "This is the classic batmobile from Batman: The Animated Series, as a kid's car.", ImageURL = (string)null, Manufacturer = "GBC", Stock = 5, BuyPrice = 300m, SellPrice = 350m },
                new Product() { ProductId = 3, ProdCatId = 1, Name = "The Tumbler", Description = "This is the batmobile from Christopher Nolan's Dark Knight Trilogy, as a kid's car.", ImageURL = "https://i.imgur.com/DDVv15J.png", Manufacturer = "GBC", Stock = 15, BuyPrice = 200m, SellPrice = 250m },
                new Product() { ProductId = 4, ProdCatId = 1, Name = "The Batman (2022) Batmobile", Description = "This is the batmobile from Matt Reeves' 2022 The Batman, as a kid's car.", ImageURL = (string)null, Manufacturer = "GBC", Stock = 15, BuyPrice = 150m, SellPrice = 200m },
                new Product() { ProductId = 5, ProdCatId = 2, Name = "Bat-Symbol Shirt", Description = "This is a shirt with the Bat-Symbol on it.", ImageURL = "https://i.imgur.com/CdlTueC.png", Manufacturer = "GBC", Stock = 20, BuyPrice = 10m, SellPrice = 15m },
                new Product() { ProductId = 6, ProdCatId = 2, Name = "Joker Shirt", Description = "This is a shirt with The Joker on it.", ImageURL = "https://i.imgur.com/KnHLQZk.png", Manufacturer = "GBC", Stock = 20, BuyPrice = 15m, SellPrice = 20m },
                new Product() { ProductId = 7, ProdCatId = 2, Name = "Harley Quinn Shirt", Description = "This is a shirt with Harley Quinn on it.", ImageURL = (string)null, Manufacturer = "Andriu", Stock = 20, BuyPrice = 20m, SellPrice = 25m },
                new Product() { ProductId = 8, ProdCatId = 2, Name = "Scarecrow Shirt", Description = "This is a shirt with Scarecrow on it.", ImageURL = (string)null, Manufacturer = "GBC", Stock = 20, BuyPrice = 15m, SellPrice = 20m },
                new Product() { ProductId = 9, ProdCatId = 3, Name = "Batarang", Description = "This is a steel replica of a Batarang. This is made for display and should not be thrown or used as a weapon.", ImageURL = "https://i.imgur.com/y3g2oOE.png", Manufacturer = "GBC", Stock = 5, BuyPrice = 5m, SellPrice = 10m },
                new Product() { ProductId = 10, ProdCatId = 3, Name = "Riddler's Staff", Description = "This is an aluminum replica of Riddler's Staff. This is made for display and should not be used as a weapon.", ImageURL = "https://i.imgur.com/GO1qH0z.png", Manufacturer = "GBC", Stock = 15, BuyPrice = 15m, SellPrice = 20m },
                new Product() { ProductId = 11, ProdCatId = 3, Name = "Shark Repellent", Description = "This is a modern take on Batman's Shark Repellent from the 1966 Batman movie. This is made for display and should not be used to repel sharks or any other creatures.", ImageURL = "https://i.imgur.com/v3fZC0J.png", Manufacturer = "GBC", Stock = 20, BuyPrice = 5m, SellPrice = 10m },
                new Product() { ProductId = 12, ProdCatId = 3, Name = "Joker's Playing Cards", Description = "This is a steel replica of The Joker's Playing Card weapons. This is made for display and should not be thrown or used as a weapon.", ImageURL = "https://i.imgur.com/U9uKlWf.png", Manufacturer = "GBC", Stock = 5, BuyPrice = 10m, SellPrice = 15m },
                new Product() { ProductId = 13, ProdCatId = 4, Name = "Frank Miller's Dark Knight Figure", Description = "This is a high quality figure of Frank Miller's Dark Knight.", ImageURL = "https://www.tftoys.ca/cdn/shop/products/McFarlaneDCMultiverseDarkKnightReturnsBatman3_large.jpg?v=1634916529", Manufacturer = "McFarlane Toys", Stock = 15, BuyPrice = 40m, SellPrice = 45m },
                new Product() { ProductId = 14, ProdCatId = 4, Name = "The Joker (1989 Version) DX Series Figure", Description = "This is a high quality figure of The Joker from Tim Burton's 1989 Batman movie.", ImageURL = "https://i.pinimg.com/474x/d0/0f/fe/d00ffeb2ef5c53614261c97d59475916.jpg", Manufacturer = "Hot Toys", Stock = 2, BuyPrice = 300m, SellPrice = 310m },
                new Product() { ProductId = 15, ProdCatId = 4, Name = "Arkham Knight Harley Quinn Figure", Description = "This is a high quality figure of Harley Quinn from the game, Batman: Arkham Knight.", ImageURL = "https://m.media-amazon.com/images/I/71oyx1LdP+L._AC_UF894,1000_QL80_.jpg", Manufacturer = "Mattel", Stock = 0, BuyPrice = 100m, SellPrice = 105m },
                new Product() { ProductId = 16, ProdCatId = 4, Name = "Batman: Hush Poison Ivy Figure", Description = "This is a high quality figure of Poison Ivy from the comic, Batman: Hush.", ImageURL = "https://i.ebayimg.com/images/g/MjgAAOSwK~pjrWuq/s-l1200.jpg", Manufacturer = "Medicom", Stock = 1, BuyPrice = 150m, SellPrice = 160m },
                new Product() { ProductId = 17, ProdCatId = 5, Name = "The Dark Knight Returns", Description = "This is Frank Miller's infamous graphic novel, The Dark Knight Returns.", ImageURL = "https://static.wikia.nocookie.net/batman/images/7/74/The_Dark_Knight_Returns.jpg", Manufacturer = "Frank Miller", Stock = 5, BuyPrice = 25m, SellPrice = 30m },
                new Product() { ProductId = 18, ProdCatId = 5, Name = "Batman: White Knight", Description = "In a world where Joker becomes sane and sues Batman for his senseless violence against the mentally ill.", ImageURL = "https://static.dc.com/dc/files/default_images/BMJKWK_01_300-001_HD_5b7f187f0c3930.50204153.jpg", Manufacturer = "Sean Murphy", Stock = 4, BuyPrice = 25m, SellPrice = 30m },
                new Product() { ProductId = 19, ProdCatId = 5, Name = "Batman: Year One", Description = "A comic that takes place during Bruce's first year as Batman. Will he be able to save Gotham?", ImageURL = "https://cdn2.penguin.com.au/covers/original/9781401207526.jpg", Manufacturer = "Frank Miller and David Mazzucchelli", Stock = 7, BuyPrice = 50m, SellPrice = 55m },
                new Product() { ProductId = 20, ProdCatId = 5, Name = "Batman: The Killing Joke", Description = "A comic that explores the possible origins of The Joker, as well as a thrilling modern day story with The Clown Prince of Crime.", ImageURL = "https://static.wikia.nocookie.net/marvel_dc/images/1/17/Batman_The_Killing_Joke.jpg", Manufacturer = "Alan Moore", Stock = 10, BuyPrice = 20m, SellPrice = 25m }

            );
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.ToTable("ProductCategory");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ProdCat)
                .HasMaxLength(60)
                .IsUnicode(false);

            entity.Property(e => e.ImageURL)
            .HasMaxLength(2083)
            .IsUnicode(false);

            entity.HasData(
                new ProductCategory() { CategoryId = 1, ProdCat = "Kid's Ride-On Cars", ImageURL = "https://www.iconpacks.net/icons/2/free-car-icon-2897-thumb.png" },
                new ProductCategory() { CategoryId = 2, ProdCat = "T-Shirts", ImageURL = "https://cdn-icons-png.flaticon.com/512/3210/3210104.png" },
                new ProductCategory() { CategoryId = 3, ProdCat = "Props/Replicas", ImageURL = "https://cdn-icons-png.flaticon.com/512/4905/4905617.png" },
                new ProductCategory() { CategoryId = 4, ProdCat = "Figures", ImageURL = "https://cdn-icons-png.flaticon.com/512/6967/6967649.png" },
                new ProductCategory() { CategoryId = 5, ProdCat = "Comics/Graphic Novels", ImageURL = "https://cdn-icons-png.flaticon.com/512/29/29302.png" }
            );
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId);

            entity.Property(e => e.Price)
                .HasColumnType("numeric(8,2)");

            entity.HasOne(e => e.ShoppingCart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(e => e.CartId);

            entity.HasOne(e => e.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(e => e.ProductId);
        });

        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.CartId);

            entity.HasOne(e => e.Customer)
               .WithOne(c => c.ShopCart)
               .HasForeignKey<ShoppingCart>(e => e.CustomerId)
               .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId);

            entity.Property(e => e.Price)
                .HasColumnType("numeric(8, 2)");

            entity.HasOne(e => e.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(e => e.OrderId);

            entity.HasOne(e => e.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(e => e.ProductId);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.Property(e => e.Total)
                .HasColumnType("numeric(10, 2)");

            entity.Property(e => e.Taxes)
                .HasColumnType("numeric(8, 2)");

            entity.HasOne(e => e.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(e => e.CustomerId);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.Property(e => e.FirstName)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsRequired();

            entity.Property(e => e.Province)
                .HasMaxLength(2)
                .IsRequired();

            entity.Property(e => e.CreditCard)
                .HasMaxLength(16)
                .IsRequired();

            entity.HasData(
                new Customer() { CustomerId = 1, FirstName = "John", LastName = "Doe", Email = "john-doe@email.com", PhoneNumber = "1234567890", Province = "ON", CreditCard = "1111222233334444" },
                new Customer() { CustomerId = 2, FirstName = "Jane", LastName = "Doe", Email = "jane-doe@email.com", PhoneNumber = "1234567890", Province = "QC", CreditCard = "1111222233334444" }
            );
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
