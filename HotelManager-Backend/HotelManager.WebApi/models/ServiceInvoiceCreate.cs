using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManager.WebApi.Models
{
    public class ServiceInvoiceCreate
    {
        public int Quantity { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid ServiceId { get; set; }
    }
}