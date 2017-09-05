/****** Object:  Database [Amilcar]    Script Date: 05/09/2017 15:00:44 ******/
CREATE DATABASE [Amilcar]
GO
ALTER DATABASE [Amilcar] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Amilcar].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Amilcar] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Amilcar] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Amilcar] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Amilcar] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Amilcar] SET ARITHABORT OFF 
GO
ALTER DATABASE [Amilcar] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Amilcar] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [Amilcar] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Amilcar] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Amilcar] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Amilcar] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Amilcar] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Amilcar] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Amilcar] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Amilcar] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Amilcar] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Amilcar] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Amilcar] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Amilcar] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Amilcar] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Amilcar] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Amilcar] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Amilcar] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Amilcar] SET RECOVERY FULL 
GO
ALTER DATABASE [Amilcar] SET  MULTI_USER 
GO
ALTER DATABASE [Amilcar] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Amilcar] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Amilcar] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Amilcar] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Amilcar', N'ON'
GO
USE [Amilcar]
GO
/****** Object:  Table [dbo].[Bodega_Detalle]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bodega_Detalle](
	[id_Bodega_Detalle] [int] NOT NULL,
	[id_bodega_productos] [int] NOT NULL,
	[id_talla] [int] NOT NULL,
	[id_color] [int] NOT NULL,
	[cantidad] [int] NOT NULL,
 CONSTRAINT [PK_Bodega_Detalle] PRIMARY KEY CLUSTERED 
(
	[id_Bodega_Detalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SalidaDetalle]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalidaDetalle](
	[idSalidaDetalle] [int] NOT NULL,
	[Id_Salida] [int] NOT NULL,
	[id_articulo] [int] NOT NULL,
	[descripcion] [nvarchar](50) NOT NULL,
	[cantidad] [int] NOT NULL,
	[id_Kardex] [int] NOT NULL,
 CONSTRAINT [PK_SalidaDetalle] PRIMARY KEY CLUSTERED 
(
	[idSalidaDetalle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Apartado]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Apartado](
	[id_apartado] [int] IDENTITY(1,1) NOT NULL,
	[fecha_plazo] [date] NOT NULL,
	[importe] [float] NOT NULL,
	[id_orden] [int] NOT NULL,
	[id_cliente] [int] NOT NULL,
 CONSTRAINT [PK_Tbl_Apartado] PRIMARY KEY CLUSTERED 
(
	[id_apartado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Articulo]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Articulo](
	[id_articulo] [int] IDENTITY(1,1) NOT NULL,
	[codigo_articulo] [nvarchar](50) NOT NULL,
	[nombre_articulo] [nvarchar](50) NOT NULL,
	[descripcion_articulo] [nvarchar](200) NOT NULL,
	[imagen] [image] NULL,
	[id_categoria] [int] NOT NULL,
	[credito_articulo] [int] NOT NULL,
	[id_Marca] [int] NOT NULL,
	[Garantia] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_Tbl_Articulo] PRIMARY KEY CLUSTERED 
(
	[id_articulo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_bodega]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_bodega](
	[id_bodega] [int] NOT NULL,
	[Descripcion] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Tbl_bodega] PRIMARY KEY CLUSTERED 
(
	[id_bodega] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_bodega_productos]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_bodega_productos](
	[id_bodega_productos1] [int] NOT NULL,
	[id_bodega] [int] NOT NULL,
	[id_articulo] [int] NOT NULL,
	[stock] [int] NOT NULL,
 CONSTRAINT [PK_Tbl_bodega_productos_1] PRIMARY KEY CLUSTERED 
(
	[id_bodega_productos1] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Categorias]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Categorias](
	[id_categoria] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](50) NOT NULL,
	[id_CatPadre] [int] NULL,
 CONSTRAINT [PK_Tbl_Categorias] PRIMARY KEY CLUSTERED 
(
	[id_categoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Clientes]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Clientes](
	[id_cliente] [int] IDENTITY(1,1) NOT NULL,
	[nombre_cliente] [nvarchar](20) NOT NULL,
	[apellidos_cliente] [nvarchar](20) NOT NULL,
	[direccion] [nvarchar](250) NOT NULL,
	[departamento] [nvarchar](20) NOT NULL,
	[telefono] [int] NOT NULL,
	[cedula] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_Clientes] PRIMARY KEY CLUSTERED 
(
	[id_cliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Color]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Color](
	[id_Color] [int] NOT NULL,
	[Descripcion] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Tbl_Color] PRIMARY KEY CLUSTERED 
(
	[id_Color] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Compra]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Compra](
	[id_compra] [int] NOT NULL,
	[id_proveedor] [int] NOT NULL,
	[fecha_compra] [date] NOT NULL,
	[fact_compra] [nvarchar](10) NOT NULL,
	[tipo_comprobante_compra] [nvarchar](50) NOT NULL,
	[iva_compra] [decimal](4, 2) NOT NULL,
	[usuario] [nchar](10) NOT NULL,
	[id_bodega] [int] NOT NULL,
 CONSTRAINT [PK_Tbl_Compra] PRIMARY KEY CLUSTERED 
(
	[id_compra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Concepto]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Concepto](
	[id_concepto] [int] NOT NULL,
	[Descripcion] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Tbl_Concepto] PRIMARY KEY CLUSTERED 
(
	[id_concepto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Configuracion]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Configuracion](
	[id_Configuracion] [int] NOT NULL,
	[Nombre] [nvarchar](50) NULL,
	[Direccion] [nvarchar](50) NULL,
	[telefono] [nvarchar](50) NULL,
	[correo] [nvarchar](50) NULL,
	[monto] [float] NULL,
	[id_sucursal] [int] NULL,
 CONSTRAINT [PK_Tbl_Configuracion] PRIMARY KEY CLUSTERED 
(
	[id_Configuracion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Credito]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Credito](
	[id_credito] [int] IDENTITY(1,1) NOT NULL,
	[id_orden] [int] NOT NULL,
	[meses_plazo] [int] NOT NULL,
	[importe] [float] NOT NULL,
	[amortizacion] [float] NOT NULL,
	[fecha_consolidacion] [date] NOT NULL,
	[cuotas] [int] NOT NULL,
	[interesPorMora] [float] NULL,
	[id_cliente] [int] NOT NULL,
 CONSTRAINT [PK_Tbl_Credito] PRIMARY KEY CLUSTERED 
(
	[id_credito] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Descripciones]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Descripciones](
	[id_descripcion] [int] NOT NULL,
	[id_categoria] [int] NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
	[valor] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Tbl_Descripciones] PRIMARY KEY CLUSTERED 
(
	[id_descripcion] ASC,
	[id_categoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Detalle_Compra]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Detalle_Compra](
	[id_detalle_compra] [int] IDENTITY(1,1) NOT NULL,
	[id_compra] [int] NOT NULL,
	[id_articulo] [int] NOT NULL,
	[id_Kardex] [int] NOT NULL,
	[cantidad] [int] NOT NULL,
	[descuento] [float] NULL,
 CONSTRAINT [PK_Tbl_Detalle_Compra] PRIMARY KEY CLUSTERED 
(
	[id_detalle_compra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Detalle_Orden]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Detalle_Orden](
	[id_detalle_orden] [int] IDENTITY(1,1) NOT NULL,
	[id_articulo] [int] NOT NULL,
	[id_orden] [int] NOT NULL,
	[cantidad] [int] NOT NULL,
	[precio_venta] [float] NOT NULL,
	[descuento] [float] NULL,
	[id_kardex] [int] NOT NULL,
 CONSTRAINT [PK_Tbl_Detalle_Orden] PRIMARY KEY CLUSTERED 
(
	[id_detalle_orden] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Devolucion]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Devolucion](
	[id_devolucion] [float] NOT NULL,
	[fecha_devolucion] [date] NOT NULL,
	[usuario] [nvarchar](10) NOT NULL,
	[id_garantia] [int] NOT NULL,
	[id_orden] [int] NOT NULL,
	[id_kardex] [int] NOT NULL,
 CONSTRAINT [PK_Tbl_Devolucion] PRIMARY KEY CLUSTERED 
(
	[id_devolucion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Empleado]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Empleado](
	[id_empleado] [nvarchar](10) NOT NULL,
	[nombre] [nvarchar](20) NOT NULL,
	[apellidos] [nvarchar](20) NOT NULL,
	[ced_empleado] [nvarchar](50) NOT NULL,
	[telefono_empleado] [nvarchar](10) NOT NULL,
	[acceso] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Tbl_Empleado] PRIMARY KEY CLUSTERED 
(
	[id_empleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_garantia]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_garantia](
	[id_garantia] [int] IDENTITY(1,1) NOT NULL,
	[cod_garantia] [nvarchar](10) NOT NULL,
	[plazo_garantia] [nvarchar](50) NOT NULL,
	[estado] [nvarchar](50) NOT NULL,
	[id_detalle_orden] [int] NOT NULL,
 CONSTRAINT [PK_Tbl_garantia] PRIMARY KEY CLUSTERED 
(
	[id_garantia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Kardex]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Kardex](
	[id_Kardex] [int] IDENTITY(1,1) NOT NULL,
	[id_articulo] [int] NOT NULL,
	[fechaKardex] [date] NOT NULL,
	[num_factura] [nvarchar](50) NOT NULL,
	[Entrada] [int] NOT NULL,
	[salida] [int] NOT NULL,
	[saldo] [int] NOT NULL,
	[precio] [float] NOT NULL,
	[costoPromedio] [float] NOT NULL,
	[usuario] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Tbl_Kardex_1] PRIMARY KEY CLUSTERED 
(
	[id_Kardex] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Marca]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Marca](
	[id_Marca] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Tbl_Marca] PRIMARY KEY CLUSTERED 
(
	[id_Marca] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Orden]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Orden](
	[id_orden] [int] IDENTITY(1,1) NOT NULL,
	[id_sucursal] [int] NULL,
	[usuario] [nvarchar](10) NOT NULL,
	[fecha_orden] [date] NOT NULL,
	[iva_orden] [decimal](4, 2) NOT NULL,
	[estado_orden] [nvarchar](20) NOT NULL,
	[tipo_orden] [nvarchar](20) NOT NULL,
	[fact_Orden] [nvarchar](10) NOT NULL,
	[id_bodega] [int] NOT NULL,
	[NombreCliente] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_Orden] PRIMARY KEY CLUSTERED 
(
	[id_orden] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Pago_Apartado]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Pago_Apartado](
	[id_pago_apartado] [int] NOT NULL,
	[id_apartado] [int] NOT NULL,
	[fecha_pago] [date] NULL,
	[Cantidad] [float] NOT NULL,
	[saldo] [float] NOT NULL,
 CONSTRAINT [PK_Tbl_Pago_Apartado] PRIMARY KEY CLUSTERED 
(
	[id_pago_apartado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Pago_Credito]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Pago_Credito](
	[id_pago_credito] [int] NOT NULL,
	[id_credito] [int] NOT NULL,
	[fecha_pago] [date] NOT NULL,
	[cantidad] [float] NOT NULL,
	[mora] [float] NULL,
	[saldo] [float] NOT NULL,
	[fecha_estimada_pago] [date] NULL,
 CONSTRAINT [PK_Tbl_Pago_Credito] PRIMARY KEY CLUSTERED 
(
	[id_pago_credito] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Presentacion]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Presentacion](
	[id_presentacion] [int] IDENTITY(1,1) NOT NULL,
	[descripcion_presentacion] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Tbl_Presentacion] PRIMARY KEY CLUSTERED 
(
	[id_presentacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Proveedor]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Tbl_Proveedor](
	[id_proveedor] [int] IDENTITY(1,1) NOT NULL,
	[razon_social] [nvarchar](50) NOT NULL,
	[categoria] [nvarchar](50) NOT NULL,
	[telefono] [varchar](10) NOT NULL,
	[Ruc] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Tbl_Proveedor] PRIMARY KEY CLUSTERED 
(
	[id_proveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Tbl_Salida]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Salida](
	[IDSalida] [int] IDENTITY(1,1) NOT NULL,
	[fecha] [date] NOT NULL,
	[IdConcepto] [int] NOT NULL,
	[IdBodega] [int] NOT NULL,
 CONSTRAINT [PK_Tbl_Salida] PRIMARY KEY CLUSTERED 
(
	[IDSalida] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Sucursal]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Sucursal](
	[id_sucursal] [int] NOT NULL,
	[Nombre] [nvarchar](50) NULL,
	[imagen] [nvarchar](50) NULL,
 CONSTRAINT [PK_Tbl_Sucursal] PRIMARY KEY CLUSTERED 
(
	[id_sucursal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tbl_Talla]    Script Date: 05/09/2017 15:00:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tbl_Talla](
	[id_Talla] [int] NOT NULL,
	[Descripcion] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Tbl_Talla] PRIMARY KEY CLUSTERED 
(
	[id_Talla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Bodega_Detalle]  WITH CHECK ADD  CONSTRAINT [FK_Bodega_Detalle_Tbl_bodega_productos] FOREIGN KEY([id_bodega_productos])
REFERENCES [dbo].[Tbl_bodega_productos] ([id_bodega_productos1])
GO
ALTER TABLE [dbo].[Bodega_Detalle] CHECK CONSTRAINT [FK_Bodega_Detalle_Tbl_bodega_productos]
GO
ALTER TABLE [dbo].[Bodega_Detalle]  WITH CHECK ADD  CONSTRAINT [FK_Bodega_Detalle_Tbl_Color] FOREIGN KEY([id_color])
REFERENCES [dbo].[Tbl_Color] ([id_Color])
GO
ALTER TABLE [dbo].[Bodega_Detalle] CHECK CONSTRAINT [FK_Bodega_Detalle_Tbl_Color]
GO
ALTER TABLE [dbo].[Bodega_Detalle]  WITH CHECK ADD  CONSTRAINT [FK_Bodega_Detalle_Tbl_Talla] FOREIGN KEY([id_talla])
REFERENCES [dbo].[Tbl_Talla] ([id_Talla])
GO
ALTER TABLE [dbo].[Bodega_Detalle] CHECK CONSTRAINT [FK_Bodega_Detalle_Tbl_Talla]
GO
ALTER TABLE [dbo].[SalidaDetalle]  WITH CHECK ADD  CONSTRAINT [FK_SalidaDetalle_Tbl_Articulo] FOREIGN KEY([id_articulo])
REFERENCES [dbo].[Tbl_Articulo] ([id_articulo])
GO
ALTER TABLE [dbo].[SalidaDetalle] CHECK CONSTRAINT [FK_SalidaDetalle_Tbl_Articulo]
GO
ALTER TABLE [dbo].[SalidaDetalle]  WITH CHECK ADD  CONSTRAINT [FK_SalidaDetalle_Tbl_Kardex] FOREIGN KEY([id_Kardex])
REFERENCES [dbo].[Tbl_Kardex] ([id_Kardex])
GO
ALTER TABLE [dbo].[SalidaDetalle] CHECK CONSTRAINT [FK_SalidaDetalle_Tbl_Kardex]
GO
ALTER TABLE [dbo].[SalidaDetalle]  WITH CHECK ADD  CONSTRAINT [FK_SalidaDetalle_Tbl_Salida] FOREIGN KEY([Id_Salida])
REFERENCES [dbo].[Tbl_Salida] ([IDSalida])
GO
ALTER TABLE [dbo].[SalidaDetalle] CHECK CONSTRAINT [FK_SalidaDetalle_Tbl_Salida]
GO
ALTER TABLE [dbo].[Tbl_Apartado]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Apartado_Tbl_Clientes] FOREIGN KEY([id_cliente])
REFERENCES [dbo].[Tbl_Clientes] ([id_cliente])
GO
ALTER TABLE [dbo].[Tbl_Apartado] CHECK CONSTRAINT [FK_Tbl_Apartado_Tbl_Clientes]
GO
ALTER TABLE [dbo].[Tbl_Apartado]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Apartado_Tbl_Orden] FOREIGN KEY([id_orden])
REFERENCES [dbo].[Tbl_Orden] ([id_orden])
GO
ALTER TABLE [dbo].[Tbl_Apartado] CHECK CONSTRAINT [FK_Tbl_Apartado_Tbl_Orden]
GO
ALTER TABLE [dbo].[Tbl_Articulo]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Articulo_Tbl_Categorias] FOREIGN KEY([id_categoria])
REFERENCES [dbo].[Tbl_Categorias] ([id_categoria])
GO
ALTER TABLE [dbo].[Tbl_Articulo] CHECK CONSTRAINT [FK_Tbl_Articulo_Tbl_Categorias]
GO
ALTER TABLE [dbo].[Tbl_Articulo]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Articulo_Tbl_Marca] FOREIGN KEY([id_categoria])
REFERENCES [dbo].[Tbl_Marca] ([id_Marca])
GO
ALTER TABLE [dbo].[Tbl_Articulo] CHECK CONSTRAINT [FK_Tbl_Articulo_Tbl_Marca]
GO
ALTER TABLE [dbo].[Tbl_bodega_productos]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_bodega_productos_Tbl_Articulo] FOREIGN KEY([id_articulo])
REFERENCES [dbo].[Tbl_Articulo] ([id_articulo])
GO
ALTER TABLE [dbo].[Tbl_bodega_productos] CHECK CONSTRAINT [FK_Tbl_bodega_productos_Tbl_Articulo]
GO
ALTER TABLE [dbo].[Tbl_bodega_productos]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_bodega_productos_Tbl_bodega] FOREIGN KEY([id_bodega])
REFERENCES [dbo].[Tbl_bodega] ([id_bodega])
GO
ALTER TABLE [dbo].[Tbl_bodega_productos] CHECK CONSTRAINT [FK_Tbl_bodega_productos_Tbl_bodega]
GO
ALTER TABLE [dbo].[Tbl_Categorias]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Categorias_Tbl_Categorias1] FOREIGN KEY([id_CatPadre])
REFERENCES [dbo].[Tbl_Categorias] ([id_categoria])
GO
ALTER TABLE [dbo].[Tbl_Categorias] CHECK CONSTRAINT [FK_Tbl_Categorias_Tbl_Categorias1]
GO
ALTER TABLE [dbo].[Tbl_Compra]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Compra_Tbl_bodega] FOREIGN KEY([id_bodega])
REFERENCES [dbo].[Tbl_bodega] ([id_bodega])
GO
ALTER TABLE [dbo].[Tbl_Compra] CHECK CONSTRAINT [FK_Tbl_Compra_Tbl_bodega]
GO
ALTER TABLE [dbo].[Tbl_Compra]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Compra_Tbl_Proveedor] FOREIGN KEY([id_proveedor])
REFERENCES [dbo].[Tbl_Proveedor] ([id_proveedor])
GO
ALTER TABLE [dbo].[Tbl_Compra] CHECK CONSTRAINT [FK_Tbl_Compra_Tbl_Proveedor]
GO
ALTER TABLE [dbo].[Tbl_Configuracion]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Configuracion_Tbl_Sucursal] FOREIGN KEY([id_sucursal])
REFERENCES [dbo].[Tbl_Sucursal] ([id_sucursal])
GO
ALTER TABLE [dbo].[Tbl_Configuracion] CHECK CONSTRAINT [FK_Tbl_Configuracion_Tbl_Sucursal]
GO
ALTER TABLE [dbo].[Tbl_Credito]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Credito_Tbl_Clientes] FOREIGN KEY([id_cliente])
REFERENCES [dbo].[Tbl_Clientes] ([id_cliente])
GO
ALTER TABLE [dbo].[Tbl_Credito] CHECK CONSTRAINT [FK_Tbl_Credito_Tbl_Clientes]
GO
ALTER TABLE [dbo].[Tbl_Credito]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Credito_Tbl_Orden] FOREIGN KEY([id_orden])
REFERENCES [dbo].[Tbl_Orden] ([id_orden])
GO
ALTER TABLE [dbo].[Tbl_Credito] CHECK CONSTRAINT [FK_Tbl_Credito_Tbl_Orden]
GO
ALTER TABLE [dbo].[Tbl_Descripciones]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Descripciones_Tbl_Categorias] FOREIGN KEY([id_categoria])
REFERENCES [dbo].[Tbl_Categorias] ([id_categoria])
GO
ALTER TABLE [dbo].[Tbl_Descripciones] CHECK CONSTRAINT [FK_Tbl_Descripciones_Tbl_Categorias]
GO
ALTER TABLE [dbo].[Tbl_Detalle_Compra]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Detalle_Compra_Tbl_Kardex] FOREIGN KEY([id_Kardex])
REFERENCES [dbo].[Tbl_Kardex] ([id_Kardex])
GO
ALTER TABLE [dbo].[Tbl_Detalle_Compra] CHECK CONSTRAINT [FK_Tbl_Detalle_Compra_Tbl_Kardex]
GO
ALTER TABLE [dbo].[Tbl_Detalle_Orden]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Detalle_Orden_Tbl_Articulo] FOREIGN KEY([id_articulo])
REFERENCES [dbo].[Tbl_Articulo] ([id_articulo])
GO
ALTER TABLE [dbo].[Tbl_Detalle_Orden] CHECK CONSTRAINT [FK_Tbl_Detalle_Orden_Tbl_Articulo]
GO
ALTER TABLE [dbo].[Tbl_Detalle_Orden]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Detalle_Orden_Tbl_Kardex] FOREIGN KEY([id_kardex])
REFERENCES [dbo].[Tbl_Kardex] ([id_Kardex])
GO
ALTER TABLE [dbo].[Tbl_Detalle_Orden] CHECK CONSTRAINT [FK_Tbl_Detalle_Orden_Tbl_Kardex]
GO
ALTER TABLE [dbo].[Tbl_Detalle_Orden]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Detalle_Orden_Tbl_Orden] FOREIGN KEY([id_orden])
REFERENCES [dbo].[Tbl_Orden] ([id_orden])
GO
ALTER TABLE [dbo].[Tbl_Detalle_Orden] CHECK CONSTRAINT [FK_Tbl_Detalle_Orden_Tbl_Orden]
GO
ALTER TABLE [dbo].[Tbl_Devolucion]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Devolucion_Tbl_garantia] FOREIGN KEY([id_garantia])
REFERENCES [dbo].[Tbl_garantia] ([id_garantia])
GO
ALTER TABLE [dbo].[Tbl_Devolucion] CHECK CONSTRAINT [FK_Tbl_Devolucion_Tbl_garantia]
GO
ALTER TABLE [dbo].[Tbl_Kardex]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Kardex_Tbl_Articulo] FOREIGN KEY([id_articulo])
REFERENCES [dbo].[Tbl_Articulo] ([id_articulo])
GO
ALTER TABLE [dbo].[Tbl_Kardex] CHECK CONSTRAINT [FK_Tbl_Kardex_Tbl_Articulo]
GO
ALTER TABLE [dbo].[Tbl_Orden]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Orden_Tbl_bodega] FOREIGN KEY([id_bodega])
REFERENCES [dbo].[Tbl_bodega] ([id_bodega])
GO
ALTER TABLE [dbo].[Tbl_Orden] CHECK CONSTRAINT [FK_Tbl_Orden_Tbl_bodega]
GO
ALTER TABLE [dbo].[Tbl_Orden]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Orden_Tbl_Sucursal] FOREIGN KEY([id_sucursal])
REFERENCES [dbo].[Tbl_Sucursal] ([id_sucursal])
GO
ALTER TABLE [dbo].[Tbl_Orden] CHECK CONSTRAINT [FK_Tbl_Orden_Tbl_Sucursal]
GO
ALTER TABLE [dbo].[Tbl_Pago_Apartado]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Pago_Apartado_Tbl_Apartado] FOREIGN KEY([id_pago_apartado])
REFERENCES [dbo].[Tbl_Apartado] ([id_apartado])
GO
ALTER TABLE [dbo].[Tbl_Pago_Apartado] CHECK CONSTRAINT [FK_Tbl_Pago_Apartado_Tbl_Apartado]
GO
ALTER TABLE [dbo].[Tbl_Pago_Credito]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Pago_Credito_Tbl_Credito] FOREIGN KEY([id_credito])
REFERENCES [dbo].[Tbl_Credito] ([id_credito])
GO
ALTER TABLE [dbo].[Tbl_Pago_Credito] CHECK CONSTRAINT [FK_Tbl_Pago_Credito_Tbl_Credito]
GO
ALTER TABLE [dbo].[Tbl_Salida]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Salida_Tbl_bodega] FOREIGN KEY([IdBodega])
REFERENCES [dbo].[Tbl_bodega] ([id_bodega])
GO
ALTER TABLE [dbo].[Tbl_Salida] CHECK CONSTRAINT [FK_Tbl_Salida_Tbl_bodega]
GO
ALTER TABLE [dbo].[Tbl_Salida]  WITH CHECK ADD  CONSTRAINT [FK_Tbl_Salida_Tbl_Concepto] FOREIGN KEY([IdConcepto])
REFERENCES [dbo].[Tbl_Concepto] ([id_concepto])
GO
ALTER TABLE [dbo].[Tbl_Salida] CHECK CONSTRAINT [FK_Tbl_Salida_Tbl_Concepto]
GO
USE [master]
GO
ALTER DATABASE [Amilcar] SET  READ_WRITE 
GO
