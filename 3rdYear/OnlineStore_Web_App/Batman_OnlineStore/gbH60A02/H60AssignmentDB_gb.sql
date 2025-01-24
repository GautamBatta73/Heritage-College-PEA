USE [master]
GO
/****** Object:  Database [H60Assignment2DB_gb]    Script Date: 2022-09-01 2:42:34 PM ******/
CREATE DATABASE [H60Assignment2DB_gb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'H60A02EmptyDB', FILENAME = N'E:\MSSQL15.MSSQLSERVER\MSSQL\DATA\H60Assignment2DB_gb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'H60A02EmptyDB_log', FILENAME = N'E:\MSSQL15.MSSQLSERVER\MSSQL\DATA\H60Assignment2DB_gb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [H60Assignment2DB_gb] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [H60Assignment2DB_gb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [H60Assignment2DB_gb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET ARITHABORT OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET RECOVERY FULL 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET  MULTI_USER 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [H60Assignment2DB_gb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [H60Assignment2DB_gb] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'H60Assignment2DB_gb', N'ON'
GO
ALTER DATABASE [H60Assignment2DB_gb] SET QUERY_STORE = OFF
GO
USE [H60Assignment2DB_gb]
GO


/****** Object:  Table [dbo].[Product]    Script Date: 2022-09-01 2:42:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProdCatId] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](200) NULL,
	[ImageURL] [varchar](2083) NULL,
	[Manufacturer] [varchar](80) NULL,
	[Stock] [int] NOT NULL,
	[BuyPrice] [numeric](8, 2) NULL,
	[SellPrice] [numeric](8, 2) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategory]    Script Date: 2022-09-01 2:42:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategory](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ProdCat] [varchar](60) NOT NULL,
	[ImageURL] [varchar](2083) NULL,
 CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
GO
/****** Object:  Index [IX_Product_ProdCatId]    Script Date: 2022-09-01 2:42:34 PM ******/
CREATE NONCLUSTERED INDEX [IX_Product_ProdCatId] ON [dbo].[Product]
(
	[ProdCatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductCategory] FOREIGN KEY([ProdCatId])
REFERENCES [dbo].[ProductCategory] ([CategoryID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_ProductCategory]
GO
USE [master]
GO
ALTER DATABASE [H60Assignment2DB_gb] SET  READ_WRITE 
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 2022-09-01 2:42:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
    [CustomerId] [int] IDENTITY(1,1) NOT NULL,
    [FirstName] [varchar](20) NOT NULL,
    [LastName] [varchar](30) NOT NULL,
    [Email] [varchar](30) NOT NULL,
    [PhoneNumber] [varchar](10) NOT NULL,
    [Province] [varchar](2) NOT NULL,
    [CreditCard] [varchar](16) NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
    [CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Create indices for optimizing search operations. Optional.
CREATE NONCLUSTERED INDEX [IX_Customer_Email] ON [dbo].[Customer] ([Email]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_Customer_PhoneNumber] ON [dbo].[Customer] ([PhoneNumber]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

GO
/*INSERTS*/
USE [H60Assignment2DB_gb]

INSERT INTO ProductCategory
(
    ProdCat,
    ImageURL
)
VALUES
('Kid''s Ride-On Cars', 'https://www.iconpacks.net/icons/2/free-car-icon-2897-thumb.png'),
('T-Shirts', 'https://cdn-icons-png.flaticon.com/512/3210/3210104.png'),
('Props/Replicas', 'https://cdn-icons-png.flaticon.com/512/4905/4905617.png'),
('Figures', 'https://cdn-icons-png.flaticon.com/512/6967/6967649.png'),
('Comics/Graphic Novels', 'https://cdn-icons-png.flaticon.com/512/29/29302.png');

INSERT INTO Product
(
    ProdCatId,
    Name,
    Description,
	ImageURL,
    Manufacturer,
    Stock,
    BuyPrice,
    SellPrice
)
VALUES
(1,
 '1989 Batmobile',
 'This is the classic batmobile from Tim Burton''s 1989 Batman, as a kid''s car.',
 'https://i.imgur.com/uOoAU3k.png',
 'GBC',
 10 ,
 250,
 300
),
(1,
 'Batman: The Animated Series Batmobile',
 'This is the classic batmobile from Batman: The Animated Series, as a kid''s car.',
 NULL,
 'GBC',
 5,
 300,
 350
),
(1,
 'The Tumbler',
 'This is the batmobile from Christopher Nolan''s Dark Knight Trilogy, as a kid''s car.',
 'https://i.imgur.com/DDVv15J.png',
 'GBC',
 15,
 200,
 250
),
(1,
 'The Batman (2022) Batmobile',
 'This is the batmobile from Matt Reeves'' 2022 The Batman, as a kid''s car.',
 NULL,
 'GBC',
 15,
 150,
 200
),
(2, 'Bat-Symbol Shirt', 'This is a shirt with the Bat-Symbol on it.', 'https://i.imgur.com/CdlTueC.png', 'GBC', 20, 10, 15),
(2, 'Joker Shirt', 'This is a shirt with The Joker on it.', 'https://i.imgur.com/KnHLQZk.png', 'GBC', 20, 15, 20),
(2, 'Harley Quinn Shirt', 'This is a shirt with Harley Quinn on it.', NULL, 'Andriu', 20, 20, 25),
(2, 'Scarecrow Shirt', 'This is a shirt with Scarecrow on it.', NULL, 'GBC', 20, 15, 20),
(3,
 'Batarang',
 'This is a steel replica of a Batarang. This is made for display and should not be thrown or used as a weapon.',
 'https://i.imgur.com/y3g2oOE.png',
 'GBC',
 5,
 5,
 10
),
(3,
 'Riddler''s Staff',
 'This is an aluminum replica of Riddler''s Staff. This is made for display and should not be used as a weapon.',
 'https://i.imgur.com/GO1qH0z.png',
 'GBC',
 15,
 15,
 20
),
(3,
 'Shark Repellent',
 'This is a modern take on Batman''s Shark Repellent from the 1966 Batman movie. This is made for display and should not be used to repel sharks or any other creatures.',
 'https://i.imgur.com/v3fZC0J.png',
 'GBC',
 20,
 5,
 10
),
(3,
 'Joker''s Playing Cards',
 'This is a steel replica of The Joker''s Playing Card weapons. This is made for display and should not be thrown or used as a weapon.',
 'https://i.imgur.com/U9uKlWf.png',
 'GBC',
 5,
 10,
 15
),
(4,
 'Frank Miller''s Dark Knight Figure',
 'This is a high quality figure of Frank Miller''s Dark Knight.',
 'https://www.tftoys.ca/cdn/shop/products/McFarlaneDCMultiverseDarkKnightReturnsBatman3_large.jpg?v=1634916529',
 'McFarlane Toys',
 15,
 40,
 45
),
(4,
 'The Joker (1989 Version) DX Series Figure',
 'This is a high quality figure of The Joker from Tim Burton''s 1989 Batman movie.',
 'https://i.pinimg.com/474x/d0/0f/fe/d00ffeb2ef5c53614261c97d59475916.jpg',
 'Hot Toys',
 2,
 300,
 310
),
(4,
 'Arkham Knight Harley Quinn Figure',
 'This is a high quality figure of Harley Quinn from the game, Batman: Arkham Knight.',
 'https://m.media-amazon.com/images/I/71oyx1LdP+L._AC_UF894,1000_QL80_.jpg',
 'Mattel',
 0,
 100,
 105
),
(4,
 'Batman: Hush Poison Ivy Figure',
 'This is a high quality figure of Poison Ivy from the comic, Batman: Hush.',
 'https://i.ebayimg.com/images/g/MjgAAOSwK~pjrWuq/s-l1200.jpg',
 'Medicom',
 1,
 150,
 160
),
(5,
 'The Dark Knight Returns',
 'This is Frank Miller''s infamous graphic novel, The Dark Knight Returns.',
 'https://static.wikia.nocookie.net/batman/images/7/74/The_Dark_Knight_Returns.jpg',
 'Frank Miller',
 5,
 25,
 30
),
(5,
 'Batman: White Knight',
 'In a world where Joker becomes sane and sues Batman for his sensless violence against the mentally ill.',
 'https://static.dc.com/dc/files/default_images/BMJKWK_01_300-001_HD_5b7f187f0c3930.50204153.jpg',
 'Sean Murphy',
 4,
 25,
 30
),
(5,
 'Batman: Year One',
 'A comic that takes place during Bruce''s first year as Batman. Will be able to save Gotham?',
 'https://cdn2.penguin.com.au/covers/original/9781401207526.jpg',
 'Frank Miller and David Mazzucchelli',
 7,
 50,
 55
),
(5,
 'Batman: The Killing Joke',
 'A comic that explores the possible origins of The Joker, as well as a thrilling modern day story with The Clown Prince of Crime.',
 'https://static.wikia.nocookie.net/marvel_dc/images/1/17/Batman_The_Killing_Joke.jpg',
 'Alan Moore',
 10,
 20,
 25
);

GO