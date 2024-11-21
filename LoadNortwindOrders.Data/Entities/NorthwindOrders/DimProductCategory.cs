using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadNortwindOrders.Data.Entities.NorthwindOrders
{
    public class DimProductCategory
    {
        
        public int IdProduct { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }

    }
}
