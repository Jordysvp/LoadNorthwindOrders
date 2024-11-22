﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadNortwindOrders.Data.Entities.Nortwind
{
    public class ServedCustomer
    {
        public int EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public int? TotalCustomersServed { get; set; }
    }
}
