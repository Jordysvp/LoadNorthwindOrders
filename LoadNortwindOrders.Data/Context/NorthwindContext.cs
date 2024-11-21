

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
        #endregion
    }
}
