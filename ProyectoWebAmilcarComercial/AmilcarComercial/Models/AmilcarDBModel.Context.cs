﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AmilcarComercial.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DBAmilcarEntities : DbContext
    {
        public DBAmilcarEntities()
            : base("name=DBAmilcarEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Bodega_Detalle> Bodega_Detalle { get; set; }
        public virtual DbSet<SalidaDetalle> SalidaDetalle { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Tbl_Apartado> Tbl_Apartado { get; set; }
        public virtual DbSet<Tbl_Articulo> Tbl_Articulo { get; set; }
        public virtual DbSet<Tbl_bodega_productos> Tbl_bodega_productos { get; set; }
        public virtual DbSet<Tbl_Categorias> Tbl_Categorias { get; set; }
        public virtual DbSet<Tbl_CategoriaTmp> Tbl_CategoriaTmp { get; set; }
        public virtual DbSet<Tbl_Clientes> Tbl_Clientes { get; set; }
        public virtual DbSet<Tbl_ClienteTmp> Tbl_ClienteTmp { get; set; }
        public virtual DbSet<Tbl_Color> Tbl_Color { get; set; }
        public virtual DbSet<Tbl_Compra> Tbl_Compra { get; set; }
        public virtual DbSet<Tbl_CompraTmp> Tbl_CompraTmp { get; set; }
        public virtual DbSet<Tbl_Concepto> Tbl_Concepto { get; set; }
        public virtual DbSet<Tbl_Configuracion> Tbl_Configuracion { get; set; }
        public virtual DbSet<Tbl_Credito> Tbl_Credito { get; set; }
        public virtual DbSet<Tbl_Departamentos> Tbl_Departamentos { get; set; }
        public virtual DbSet<Tbl_Descripciones> Tbl_Descripciones { get; set; }
        public virtual DbSet<Tbl_DescripcionValores> Tbl_DescripcionValores { get; set; }
        public virtual DbSet<Tbl_Detalle_Compra> Tbl_Detalle_Compra { get; set; }
        public virtual DbSet<Tbl_Detalle_Orden> Tbl_Detalle_Orden { get; set; }
        public virtual DbSet<Tbl_DetalleDevolucionCliente> Tbl_DetalleDevolucionCliente { get; set; }
        public virtual DbSet<Tbl_DetalleDevolucionProveedor> Tbl_DetalleDevolucionProveedor { get; set; }
        public virtual DbSet<Tbl_DevolucionCliente> Tbl_DevolucionCliente { get; set; }
        public virtual DbSet<Tbl_DevolucionDetalleTmp> Tbl_DevolucionDetalleTmp { get; set; }
        public virtual DbSet<Tbl_DevolucionProveedor> Tbl_DevolucionProveedor { get; set; }
        public virtual DbSet<Tbl_DevolucionTmp> Tbl_DevolucionTmp { get; set; }
        public virtual DbSet<Tbl_garantia> Tbl_garantia { get; set; }
        public virtual DbSet<Tbl_ImgTamporal> Tbl_ImgTamporal { get; set; }
        public virtual DbSet<Tbl_Kardex> Tbl_Kardex { get; set; }
        public virtual DbSet<Tbl_Marca> Tbl_Marca { get; set; }
        public virtual DbSet<Tbl_Orden> Tbl_Orden { get; set; }
        public virtual DbSet<Tbl_OrdenTmp> Tbl_OrdenTmp { get; set; }
        public virtual DbSet<Tbl_Pago_Apartado> Tbl_Pago_Apartado { get; set; }
        public virtual DbSet<Tbl_Pago_Credito> Tbl_Pago_Credito { get; set; }
        public virtual DbSet<Tbl_Presentacion> Tbl_Presentacion { get; set; }
        public virtual DbSet<Tbl_Proveedor> Tbl_Proveedor { get; set; }
        public virtual DbSet<Tbl_ProveedorTmp> Tbl_ProveedorTmp { get; set; }
        public virtual DbSet<Tbl_Salida> Tbl_Salida { get; set; }
        public virtual DbSet<Tbl_Sucursal> Tbl_Sucursal { get; set; }
        public virtual DbSet<Tbl_Talla> Tbl_Talla { get; set; }
    }
}
