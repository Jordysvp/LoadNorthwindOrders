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
        public DbSet<DimEmployee> DimEmployees { get; set; }
        public DbSet<DimProductCategory> DimProductCategories { get; set; }
        public DbSet<DimCustomer> DimCustomers { get; set; }
        public DbSet<DimShipper> DimShippers { get; set; }
        #endregion
    }
}
