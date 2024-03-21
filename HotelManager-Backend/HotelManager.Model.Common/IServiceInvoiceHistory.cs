using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Model.Common
{
    public interface IServiceInvoiceHistory
    {
        Guid Id { get; set; }
        string ServiceName { get; set; }
        int Quantity { get; set; }
        decimal Price { get; set; }
        DateTime DateCreated { get; set; }
    }
}
