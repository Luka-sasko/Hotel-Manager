using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Model.Common
{
    public interface IReceipt
    {
         Guid Id { get; set; }
         decimal TotalPrice { get; set; }
         bool IsPaid { get; set; }
         Guid ReservationId { get; set; }
         Guid? DiscountId { get; set; }
         Guid CreatedBy { get; set; }
         Guid UpdatedBy { get; set; }
         DateTime DateCreated { get; set; }
         DateTime DateUpdated { get; set; }
         bool IsActive { get; set; }
         string InvoiceNumber { get; set; }
    }
}
