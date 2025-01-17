USE [master]
GO
/****** Object:  Database [testdb]    Script Date: 10/20/2024 1:51:00 AM ******/
CREATE DATABASE [testdb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'testdb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\testdb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'testdb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\testdb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [testdb] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [testdb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [testdb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [testdb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [testdb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [testdb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [testdb] SET ARITHABORT OFF 
GO
ALTER DATABASE [testdb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [testdb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [testdb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [testdb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [testdb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [testdb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [testdb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [testdb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [testdb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [testdb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [testdb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [testdb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [testdb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [testdb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [testdb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [testdb] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [testdb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [testdb] SET RECOVERY FULL 
GO
ALTER DATABASE [testdb] SET  MULTI_USER 
GO
ALTER DATABASE [testdb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [testdb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [testdb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [testdb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [testdb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [testdb] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'testdb', N'ON'
GO
ALTER DATABASE [testdb] SET QUERY_STORE = OFF
GO
USE [testdb]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 10/20/2024 1:51:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cuentas]    Script Date: 10/20/2024 1:51:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cuentas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NumeroCuenta] [nvarchar](max) NOT NULL,
	[TipoCuenta] [nvarchar](max) NOT NULL,
	[SaldoInicial] [decimal](18, 2) NOT NULL,
	[Estado] [bit] NOT NULL,
	[ClienteId] [int] NOT NULL,
 CONSTRAINT [PK_Cuentas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Movimientos]    Script Date: 10/20/2024 1:51:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movimientos](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Fecha] [datetime2](7) NOT NULL,
	[TipoMovimiento] [nvarchar](max) NOT NULL,
	[Valor] [decimal](18, 2) NOT NULL,
	[Saldo] [decimal](18, 2) NOT NULL,
	[CuentaId] [int] NOT NULL,
 CONSTRAINT [PK_Movimientos] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Personas]    Script Date: 10/20/2024 1:51:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Personas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Genero] [nvarchar](max) NOT NULL,
	[Edad] [int] NOT NULL,
	[Identificacion] [nvarchar](max) NOT NULL,
	[Direccion] [nvarchar](max) NOT NULL,
	[Telefono] [nvarchar](max) NOT NULL,
	[Discriminator] [nvarchar](max) NOT NULL,
	[ClienteId] [int] NULL,
	[Contrasena] [nvarchar](max) NULL,
	[Estado] [bit] NULL,
 CONSTRAINT [PK_Personas] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241017163206_CreateInitial', N'6.0.35')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241017164508_AddUniqueConstraintToClienteID', N'6.0.35')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241018195659_InitialCreate', N'6.0.35')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241018213036_UpdateRelationShipCuentaPersona', N'6.0.35')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20241018213808_UpdateEstadoFieldCuentaTable', N'6.0.35')
GO
SET IDENTITY_INSERT [dbo].[Cuentas] ON 

INSERT [dbo].[Cuentas] ([Id], [NumeroCuenta], [TipoCuenta], [SaldoInicial], [Estado], [ClienteId]) VALUES (2, N'12345678', N'Ahorro', CAST(10000.00 AS Decimal(18, 2)), 0, 4)
INSERT [dbo].[Cuentas] ([Id], [NumeroCuenta], [TipoCuenta], [SaldoInicial], [Estado], [ClienteId]) VALUES (3, N'56789098', N'Ahorro', CAST(7000.00 AS Decimal(18, 2)), 0, 1)
INSERT [dbo].[Cuentas] ([Id], [NumeroCuenta], [TipoCuenta], [SaldoInicial], [Estado], [ClienteId]) VALUES (4, N'Patch12345', N'Ahorro', CAST(18000.00 AS Decimal(18, 2)), 1, 2)
INSERT [dbo].[Cuentas] ([Id], [NumeroCuenta], [TipoCuenta], [SaldoInicial], [Estado], [ClienteId]) VALUES (5, N'12456577', N'Corriente', CAST(2000.00 AS Decimal(18, 2)), 1, 4)
INSERT [dbo].[Cuentas] ([Id], [NumeroCuenta], [TipoCuenta], [SaldoInicial], [Estado], [ClienteId]) VALUES (6, N'12457801', N'Corriente', CAST(16500.00 AS Decimal(18, 2)), 1, 5)
INSERT [dbo].[Cuentas] ([Id], [NumeroCuenta], [TipoCuenta], [SaldoInicial], [Estado], [ClienteId]) VALUES (8, N'patch78981', N'Ahorro', CAST(9990.00 AS Decimal(18, 2)), 0, 3)
SET IDENTITY_INSERT [dbo].[Cuentas] OFF
GO
SET IDENTITY_INSERT [dbo].[Movimientos] ON 

INSERT [dbo].[Movimientos] ([Id], [Fecha], [TipoMovimiento], [Valor], [Saldo], [CuentaId]) VALUES (1, CAST(N'2024-10-19T05:50:38.4696500' AS DateTime2), N'Deposito', CAST(180.00 AS Decimal(18, 2)), CAST(10180.00 AS Decimal(18, 2)), 2)
INSERT [dbo].[Movimientos] ([Id], [Fecha], [TipoMovimiento], [Valor], [Saldo], [CuentaId]) VALUES (2, CAST(N'2024-10-19T05:50:56.9595768' AS DateTime2), N'Retiro', CAST(100.00 AS Decimal(18, 2)), CAST(10080.00 AS Decimal(18, 2)), 2)
INSERT [dbo].[Movimientos] ([Id], [Fecha], [TipoMovimiento], [Valor], [Saldo], [CuentaId]) VALUES (3, CAST(N'2024-10-19T08:07:16.8489740' AS DateTime2), N'Deposito', CAST(1000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), 3)
INSERT [dbo].[Movimientos] ([Id], [Fecha], [TipoMovimiento], [Valor], [Saldo], [CuentaId]) VALUES (5, CAST(N'2024-10-17T16:19:59.1636413' AS DateTime2), N'Deposito', CAST(1000.00 AS Decimal(18, 2)), CAST(16500.00 AS Decimal(18, 2)), 6)
INSERT [dbo].[Movimientos] ([Id], [Fecha], [TipoMovimiento], [Valor], [Saldo], [CuentaId]) VALUES (6, CAST(N'2024-10-17T16:20:36.9266263' AS DateTime2), N'Retiro', CAST(80.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), 2)
INSERT [dbo].[Movimientos] ([Id], [Fecha], [TipoMovimiento], [Valor], [Saldo], [CuentaId]) VALUES (7, CAST(N'2024-10-17T16:21:32.4608729' AS DateTime2), N'Deposito', CAST(215.00 AS Decimal(18, 2)), CAST(2000.00 AS Decimal(18, 2)), 5)
SET IDENTITY_INSERT [dbo].[Movimientos] OFF
GO
SET IDENTITY_INSERT [dbo].[Personas] ON 

INSERT [dbo].[Personas] ([Id], [Nombre], [Genero], [Edad], [Identificacion], [Direccion], [Telefono], [Discriminator], [ClienteId], [Contrasena], [Estado]) VALUES (1, N'Josue', N'Masculino', 26, N'1171178892', N'Guadalupe, San Jose', N'86390470', N'Cliente', 1, N'1234', 1)
INSERT [dbo].[Personas] ([Id], [Nombre], [Genero], [Edad], [Identificacion], [Direccion], [Telefono], [Discriminator], [ClienteId], [Contrasena], [Estado]) VALUES (2, N'Pancho', N'Masculino', 20, N'118987251', N'Calle 123', N'87261890', N'Persona', NULL, NULL, NULL)
INSERT [dbo].[Personas] ([Id], [Nombre], [Genero], [Edad], [Identificacion], [Direccion], [Telefono], [Discriminator], [ClienteId], [Contrasena], [Estado]) VALUES (3, N'Marianela Montalvo', N'Femenino', 30, N'118227812', N'Amazonas y NNUU', N'02091920', N'Cliente', 2, N'5678', 1)
INSERT [dbo].[Personas] ([Id], [Nombre], [Genero], [Edad], [Identificacion], [Direccion], [Telefono], [Discriminator], [ClienteId], [Contrasena], [Estado]) VALUES (5, N'NameTest', N'Femenino', 32, N'889876567', N'Direcction test', N'128709281', N'Cliente', 3, N'4523', 1)
INSERT [dbo].[Personas] ([Id], [Nombre], [Genero], [Edad], [Identificacion], [Direccion], [Telefono], [Discriminator], [ClienteId], [Contrasena], [Estado]) VALUES (6, N'NameTest2', N'Femenino', 30, N'115627192', N'Direcction test 2', N'128701929011', N'Cliente', 4, N'7777', 1)
INSERT [dbo].[Personas] ([Id], [Nombre], [Genero], [Edad], [Identificacion], [Direccion], [Telefono], [Discriminator], [ClienteId], [Contrasena], [Estado]) VALUES (7, N'testPatch', N'Masculino', 18, N'1221212', N'Test after put test', N'128709281', N'Cliente', 5, N'4523', 1)
INSERT [dbo].[Personas] ([Id], [Nombre], [Genero], [Edad], [Identificacion], [Direccion], [Telefono], [Discriminator], [ClienteId], [Contrasena], [Estado]) VALUES (9, N'testPatch Repositorio', N'Masculino', 27, N'112451627', N'Test Repositorio direccion', N'123344', N'Cliente', 6, N'6754', 1)
INSERT [dbo].[Personas] ([Id], [Nombre], [Genero], [Edad], [Identificacion], [Direccion], [Telefono], [Discriminator], [ClienteId], [Contrasena], [Estado]) VALUES (10, N'Juan Pérez', N'Masculino', 30, N'12345678', N'Calle Falsa 123', N'987654321', N'Cliente', 10, N'password', 1)
INSERT [dbo].[Personas] ([Id], [Nombre], [Genero], [Edad], [Identificacion], [Direccion], [Telefono], [Discriminator], [ClienteId], [Contrasena], [Estado]) VALUES (11, N'Juan Pérez', N'Masculino', 30, N'12345678', N'Calle Falsa 123', N'987654321', N'Cliente', 8, N'password', 1)
SET IDENTITY_INSERT [dbo].[Personas] OFF
GO
/****** Object:  Index [IX_Cuentas_ClienteId]    Script Date: 10/20/2024 1:51:00 AM ******/
CREATE NONCLUSTERED INDEX [IX_Cuentas_ClienteId] ON [dbo].[Cuentas]
(
	[ClienteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Movimientos_CuentaId]    Script Date: 10/20/2024 1:51:00 AM ******/
CREATE NONCLUSTERED INDEX [IX_Movimientos_CuentaId] ON [dbo].[Movimientos]
(
	[CuentaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [AK_Personas_ClienteId]    Script Date: 10/20/2024 1:51:00 AM ******/
ALTER TABLE [dbo].[Personas] ADD  CONSTRAINT [AK_Personas_ClienteId] UNIQUE NONCLUSTERED 
(
	[ClienteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Personas_ClienteId]    Script Date: 10/20/2024 1:51:00 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Personas_ClienteId] ON [dbo].[Personas]
(
	[ClienteId] ASC
)
WHERE ([ClienteId] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Personas] ADD  DEFAULT (N'Cliente') FOR [Discriminator]
GO
ALTER TABLE [dbo].[Cuentas]  WITH CHECK ADD  CONSTRAINT [FK_Cuentas_Personas_ClienteId] FOREIGN KEY([ClienteId])
REFERENCES [dbo].[Personas] ([ClienteId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Cuentas] CHECK CONSTRAINT [FK_Cuentas_Personas_ClienteId]
GO
ALTER TABLE [dbo].[Movimientos]  WITH CHECK ADD  CONSTRAINT [FK_Movimientos_Cuentas_CuentaId] FOREIGN KEY([CuentaId])
REFERENCES [dbo].[Cuentas] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Movimientos] CHECK CONSTRAINT [FK_Movimientos_Cuentas_CuentaId]
GO
USE [master]
GO
ALTER DATABASE [testdb] SET  READ_WRITE 
GO
