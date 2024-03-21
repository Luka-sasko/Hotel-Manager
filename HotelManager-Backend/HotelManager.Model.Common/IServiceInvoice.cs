using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Model.Common
{
    public interface IServiceInvoice
    {
         Guid Id { get; set; }
         int NumberOfService { get; set; }
         Guid InvoiceId { get; set; }
         Guid ServiceId { get; set; }
         Guid CreatedBy { get; set; }
         Guid UpdatedBy { get; set; }
         DateTime DateCreated { get; set; }
         DateTime DateUpdated { get; set; }
         bool IsActive { get; set; }
    }
}
