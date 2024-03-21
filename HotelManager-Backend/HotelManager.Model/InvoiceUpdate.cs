using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Model
{
    public class InvoiceUpdate
    {
        public InvoiceUpdate() { }
        public decimal TotalPrice { get; set; }
        public Guid UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
