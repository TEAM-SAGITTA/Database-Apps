USE [master]
GO
/****** Object:  Database [TeamWork]    Script Date: 06-03-2015 17:49:50 ******/
CREATE DATABASE [TeamWork]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TeamWork', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\TeamWork.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TeamWork_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\TeamWork_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [TeamWork] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TeamWork].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TeamWork] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TeamWork] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TeamWork] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TeamWork] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TeamWork] SET ARITHABORT OFF 
GO
ALTER DATABASE [TeamWork] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TeamWork] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TeamWork] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TeamWork] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TeamWork] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TeamWork] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TeamWork] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TeamWork] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TeamWork] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TeamWork] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TeamWork] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TeamWork] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TeamWork] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TeamWork] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TeamWork] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TeamWork] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TeamWork] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TeamWork] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [TeamWork] SET  MULTI_USER 
GO
ALTER DATABASE [TeamWork] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TeamWork] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TeamWork] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TeamWork] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [TeamWork] SET DELAYED_DURABILITY = DISABLED 
GO
USE [TeamWork]
GO
/****** Object:  Table [dbo].[Measures]    Script Date: 06-03-2015 17:49:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Measures](
	[ID] [int] IDENTITY(100,100) NOT NULL,
	[Measure Name] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Products]    Script Date: 06-03-2015 17:49:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VendorID] [int] NOT NULL,
	[Product Name] [nvarchar](max) NOT NULL,
	[MeasureID] [int] NOT NULL,
	[Price] [money] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Vendors]    Script Date: 06-03-2015 17:49:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendors](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Vendor Name] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Measures] ON 

INSERT [dbo].[Measures] ([ID], [Measure Name]) VALUES (100, N'g')
INSERT [dbo].[Measures] ([ID], [Measure Name]) VALUES (200, N'kg')
INSERT [dbo].[Measures] ([ID], [Measure Name]) VALUES (300, N'ml')
INSERT [dbo].[Measures] ([ID], [Measure Name]) VALUES (400, N'l')
INSERT [dbo].[Measures] ([ID], [Measure Name]) VALUES (500, N'package')
INSERT [dbo].[Measures] ([ID], [Measure Name]) VALUES (600, N'bottle')
SET IDENTITY_INSERT [dbo].[Measures] OFF
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ID], [VendorID], [Product Name], [MeasureID], [Price]) VALUES (1, 1, N'Beer "Zagorka"', 600, 2.5600)
INSERT [dbo].[Products] ([ID], [VendorID], [Product Name], [MeasureID], [Price]) VALUES (2, 8, N'Coffee "3v1"', 500, 0.3400)
INSERT [dbo].[Products] ([ID], [VendorID], [Product Name], [MeasureID], [Price]) VALUES (3, 2, N'Chocolate "Milka"', 100, 1.8900)
INSERT [dbo].[Products] ([ID], [VendorID], [Product Name], [MeasureID], [Price]) VALUES (4, 5, N'Chips "Chippi"', 100, 2.3800)
INSERT [dbo].[Products] ([ID], [VendorID], [Product Name], [MeasureID], [Price]) VALUES (5, 6, N'Coca-Cola', 400, 2.2500)
INSERT [dbo].[Products] ([ID], [VendorID], [Product Name], [MeasureID], [Price]) VALUES (6, 10, N'Peanuts', 100, 1.2600)
INSERT [dbo].[Products] ([ID], [VendorID], [Product Name], [MeasureID], [Price]) VALUES (7, 9, N'Rakia', 300, 8.6900)
SET IDENTITY_INSERT [dbo].[Products] OFF
SET IDENTITY_INSERT [dbo].[Vendors] ON 

INSERT [dbo].[Vendors] ([ID], [Vendor Name]) VALUES (1, N'Zagorka')
INSERT [dbo].[Vendors] ([ID], [Vendor Name]) VALUES (2, N'Nestle')
INSERT [dbo].[Vendors] ([ID], [Vendor Name]) VALUES (3, N'Lindt')
INSERT [dbo].[Vendors] ([ID], [Vendor Name]) VALUES (4, N'Bulgartabak')
INSERT [dbo].[Vendors] ([ID], [Vendor Name]) VALUES (5, N'Chipi')
INSERT [dbo].[Vendors] ([ID], [Vendor Name]) VALUES (6, N'Coca Cola')
INSERT [dbo].[Vendors] ([ID], [Vendor Name]) VALUES (7, N'Bankia')
INSERT [dbo].[Vendors] ([ID], [Vendor Name]) VALUES (8, N'NesCafe')
INSERT [dbo].[Vendors] ([ID], [Vendor Name]) VALUES (9, N'Vinprom Karnobat')
INSERT [dbo].[Vendors] ([ID], [Vendor Name]) VALUES (10, N'Detelina')
SET IDENTITY_INSERT [dbo].[Vendors] OFF
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Product_Measure] FOREIGN KEY([MeasureID])
REFERENCES [dbo].[Measures] ([ID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Product_Measure]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Product_Vendor] FOREIGN KEY([VendorID])
REFERENCES [dbo].[Vendors] ([ID])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Product_Vendor]
GO
USE [master]
GO
ALTER DATABASE [TeamWork] SET  READ_WRITE 
GO
