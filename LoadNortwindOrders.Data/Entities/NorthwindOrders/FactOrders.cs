using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadNortwindOrders.Data.Entities.NorthwindOrders
{
    public class FactOrders
    {
        [Key]
        public int Order { get; set; }
        public int IdCustomer { get; set; }
        public int IdEmployee { get; set; }

        public int IdFecha { get; set; }
        public int IdProducto { get; set; }
        public int IdShipper { get; set; }
        public int CantidadVendida { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal TotalVenta { get; set; }
    }
}
