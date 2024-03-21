using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Model
{
    public class InvoicePaid
    {
        public bool IsPaid { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
