﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
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
        public virtual DbSet<Tbl_bodega> Tbl_bodega { get; set; }
        public virtual DbSet<Tbl_bodega_productos> Tbl_bodega_productos { get; set; }
        public virtual DbSet<Tbl_Categorias> Tbl_Categorias { get; set; }
        public virtual DbSet<Tbl_Clientes> Tbl_Clientes { get; set; }
        public virtual DbSet<Tbl_Color> Tbl_Color { get; set; }
        public virtual DbSet<Tbl_Compra> Tbl_Compra { get; set; }
        public virtual DbSet<Tbl_Concepto> Tbl_Concepto { get; set; }
        public virtual DbSet<Tbl_Configuracion> Tbl_Configuracion { get; set; }
        public virtual DbSet<Tbl_Credito> Tbl_Credito { get; set; }
        public virtual DbSet<Tbl_Descripciones> Tbl_Descripciones { get; set; }
        public virtual DbSet<Tbl_DescripcionValores> Tbl_DescripcionValores { get; set; }
        public virtual DbSet<Tbl_Detalle_Compra> Tbl_Detalle_Compra { get; set; }
        public virtual DbSet<Tbl_Detalle_Orden> Tbl_Detalle_Orden { get; set; }
        public virtual DbSet<Tbl_Devolucion> Tbl_Devolucion { get; set; }
        public virtual DbSet<Tbl_garantia> Tbl_garantia { get; set; }
        public virtual DbSet<Tbl_Kardex> Tbl_Kardex { get; set; }
        public virtual DbSet<Tbl_Marca> Tbl_Marca { get; set; }
        public virtual DbSet<Tbl_Orden> Tbl_Orden { get; set; }
        public virtual DbSet<Tbl_Pago_Apartado> Tbl_Pago_Apartado { get; set; }
        public virtual DbSet<Tbl_Pago_Credito> Tbl_Pago_Credito { get; set; }
        public virtual DbSet<Tbl_Presentacion> Tbl_Presentacion { get; set; }
        public virtual DbSet<Tbl_Proveedor> Tbl_Proveedor { get; set; }
        public virtual DbSet<Tbl_Salida> Tbl_Salida { get; set; }
        public virtual DbSet<Tbl_Sucursal> Tbl_Sucursal { get; set; }
        public virtual DbSet<Tbl_Talla> Tbl_Talla { get; set; }
        public virtual DbSet<Tbl_CategoriaTmp> Tbl_CategoriaTmp { get; set; }
        public virtual DbSet<Tbl_Departamentos> Tbl_Departamentos { get; set; }
    }
}
