using LoadNortwindOrders.Data.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadNortwindOrders.Data.Interfaces
{
    public interface IDataServiceNortwindOrders
    {
        Task<OperactionResult> LoadDHW();

    }
}
