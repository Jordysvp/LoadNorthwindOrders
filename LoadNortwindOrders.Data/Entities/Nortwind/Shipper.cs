using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadNortwindOrders.Data.Entities.Nortwind
{
    public class Shipper
    {
        public int ShipperID { get; set; }
        public string? CompanyName { get; set; }
        public string? Phone { get; set; }
    }
}
