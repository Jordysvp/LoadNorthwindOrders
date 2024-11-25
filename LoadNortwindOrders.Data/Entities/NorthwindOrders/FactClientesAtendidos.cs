using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadNortwindOrders.Data.Entities.NorthwindOrders
{
    [Table("FactClientesAtendidos", Schema = "dbo")]
    public class FactClientesAtendidos
    {
        [Key]
        
        public int IdClienteAtendido { get; set; }

        public int? IdEmployee { get; set; }

        public int? TotalClientes { get; set; }
    }
}
