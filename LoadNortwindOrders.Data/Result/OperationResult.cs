﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadNortwindOrders.Data.Result
{
    public class OperactionResult
    {
        public OperactionResult()
        {
            this.Success = true;
        }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}