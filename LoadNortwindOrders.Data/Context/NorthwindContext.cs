

using LoadNortwindOrders.Data.Entities.Nortwind;
using Microsoft.EntityFrameworkCore;

namespace LoadNortwindOrders.Data.Context
{
    public partial class NorthwindContext : DbContext
    {
        public NorthwindContext(DbContextOptions<NorthwindContext> options) : base(options) { }


        #region db sets
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Shipper> Shippers { get; set; }
        public DbSet<Vwventa> Vwventas { get; set; }
        public DbSet<ServedCustomer> ServedCustomer {  get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.UnitPrice)
                .HasPrecision(18, 4);

            

            modelBuilder.Entity<Vwventa>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("VWVentas", "dbo");


            });

            modelBuilder.Entity<ServedCustomer>(entity =>
            {
                entity
                    .HasNoKey()
                    .ToView("VW_ServedCustomers", "dbo");


            });
        }

        
    }
}
