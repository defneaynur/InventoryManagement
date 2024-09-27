
--CREATE DATABASE SCRIPT
CREATE DATABASE [InventoryManagement]


--CREATE TABLES SCRIPT
USE [InventoryManagement]
GO

/****** Object:  Table [dbo].[Categories]    Script Date: 9/26/2024 9:01:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Categories](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](50) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[Created] [datetime] NULL,
	[Creator] [varchar](50) NULL,
	[Changed] [datetime] NULL,
	[Changer] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[PriceHistoryLog]    Script Date: 9/26/2024 9:01:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PriceHistoryLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[CostPrice] [decimal](10, 2) NULL,
	[OldSalePrice] [decimal](10, 2) NULL,
	[CurrentSalePrice] [decimal](10, 2) NULL,
	[Note] [nvarchar](100) NULL,
	[Created] [datetime] NULL,
	[Creator] [varchar](50) NULL,
 CONSTRAINT [PK__PriceHis__3214EC07C1CA3B11] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Products]    Script Date: 9/26/2024 9:01:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Products](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CategoryId] [bigint] NOT NULL,
	[ProductName] [nvarchar](50) NOT NULL,
	[Quantity] [int] NOT NULL,
	[CostPrice] [float] NULL,
	[SalePrice] [float] NOT NULL,
	[Description] [nvarchar](200) NULL,
	[MinQuantityValue] [int] NULL,
	[MaxCapacity] [int] NULL,
	[Created] [datetime] NULL,
	[Creator] [varchar](50) NULL,
	[Changed] [datetime] NULL,
	[Changer] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[StockMovements]    Script Date: 9/26/2024 9:01:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[StockMovements](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[OldQuantity] [int] NULL,
	[CurrentQuantity] [int] NOT NULL,
	[MovementType] [varchar](10) NULL,
	[Note] [nvarchar](100) NULL,
	[Created] [datetime] NULL,
	[Creator] [varchar](50) NULL,
 CONSTRAINT [PK_StockMovementLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 9/26/2024 9:01:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[Created] [datetime] NULL,
	[Creator] [varchar](50) NULL,
	[Changed] [datetime] NULL,
	[Changer] [varchar](50) NULL,
 CONSTRAINT [PK__Users__3214EC07BBCB1952] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ__Users__536C85E4EB049987] UNIQUE NONCLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ__Users__A9D10534EF82A3C7] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF_Categories_Created]  DEFAULT (getdate()) FOR [Created]
GO

ALTER TABLE [dbo].[PriceHistoryLog] ADD  CONSTRAINT [DF_PriceHistoryLog_Created]  DEFAULT (getdate()) FOR [Created]
GO

ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_Created]  DEFAULT (getdate()) FOR [Created]
GO

ALTER TABLE [dbo].[StockMovements] ADD  CONSTRAINT [DF_StockMovementLog_Created]  DEFAULT (getdate()) FOR [Created]
GO

ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Created]  DEFAULT (getdate()) FOR [Created]
GO

ALTER TABLE [dbo].[PriceHistoryLog]  WITH CHECK ADD  CONSTRAINT [FK_PriceHistoryLog_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO

ALTER TABLE [dbo].[PriceHistoryLog] CHECK CONSTRAINT [FK_PriceHistoryLog_Products]
GO

ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
GO

ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories]
GO

ALTER TABLE [dbo].[StockMovements]  WITH CHECK ADD  CONSTRAINT [FK_StockMovementLog_Products] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
GO

ALTER TABLE [dbo].[StockMovements] CHECK CONSTRAINT [FK_StockMovementLog_Products]
GO

