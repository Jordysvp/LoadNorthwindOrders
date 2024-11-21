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
        public DbSet<DimProductCategory> DimProductCategorie { get; set; }
        public DbSet<DimCustomer> DimCustomer { get; set; }
        public DbSet<DimShipper> DimShipper { get; set; }
        #endregion
    }
}
