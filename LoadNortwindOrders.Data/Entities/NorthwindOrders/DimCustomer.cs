using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadNortwindOrders.Data.Entities.NorthwindOrders
{
    public class DimCustomer
    {
        
        public string? IdCustomer { get; set; }
        public string? CustomerName { get; set; }
        public string? City { get; set; }

        public string? Country { get; set; }

        public string? Phone { get; set; }
    }
}
