﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadNortwindOrders.Data.Entities.NorthwindOrders
{
    public class DimShipper
    {
        
        public int IdShipper{ get; set; }
        public string? CompanyName { get; set; }
        public string? Phone { get; set; }
    }
}
