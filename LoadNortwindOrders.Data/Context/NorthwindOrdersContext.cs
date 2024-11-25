using LoadNortwindOrders.Data.Entities.NorthwindOrders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadNortwindOrders.Data.Context
{
    public class NorthwindOrdersContext : DbContext
    {
        public NorthwindOrdersContext(DbContextOptions<NorthwindOrdersContext> options) : base(options)
        {

        }

        #region "Db Sets"
        public DbSet<DimEmployee> DimEmployee { get; set; }
        public DbSet<DimProductCategory> DimProductCategory { get; set; }
        public DbSet<DimCustomer> DimCustomer { get; set; }
        public DbSet<DimShipper> DimShipper { get; set; }
        public DbSet<FactOrders> FactOrders { get; set; }
        public DbSet<FactClientesAtendidos> FactClientesAtendidos { get; set; }
        public DbSet<DimDate> DimDate { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            



            modelBuilder.Entity<FactOrders>()
                .Property(f => f.TotalVenta)
                .HasColumnType("decimal(18,2)");

        }

    }
}
