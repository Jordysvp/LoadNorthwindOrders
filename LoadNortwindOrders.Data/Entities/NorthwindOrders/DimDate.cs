using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadNortwindOrders.Data.Entities.NorthwindOrders
{
    public class DimDate
    {
        [Key] 
        public int DateID { get; set; }

        public DateTime FullDate { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }
    }
}
