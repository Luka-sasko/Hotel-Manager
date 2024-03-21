using HotelManager.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Model
{
    public class ServiceInvoice : IServiceInvoice
    {
        public Guid Id { get; set; }
        public int NumberOfService { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set;}
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set;}
        public bool IsActive { get; set; }
    }
}
