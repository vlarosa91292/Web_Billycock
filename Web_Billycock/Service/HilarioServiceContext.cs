using Billycock.Models;
using Billycock.Models.Hilario;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Billycock.Data;

namespace Billycock.Data
{
    public class HilarioServiceContext : DbContext
    {
        private readonly IDBSqlRepository _configuration;

        public HilarioServiceContext(IDBSqlRepository configuration)
        {
            _configuration = configuration;
        }

        protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(await _configuration.GetConnectionStrings("H"), 
                providerOptions => providerOptions.EnableRetryOnFailure());
        }

        public DbSet<Producto> PRODUCTO { get; set; }
        public DbSet<Proveedor> PROVEEDOR { get; set; }
        public DbSet<Oferta> OFERTA { get; set; }
        public DbSet<Linea> LINEA { get; set; }
        //public DbSet<Venta.Cabecera> VENTA_CABECERA { get; set; }
        //public DbSet<Venta.Detalle> VENTA_DETALLE { get; set; }

        public DbSet<Historia> HISTORIA { get; set; }
        public DbSet<Estado> ESTADO { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
