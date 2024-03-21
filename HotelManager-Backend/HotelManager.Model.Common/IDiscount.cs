using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Model.Common
{
    public interface IDiscount
    {
        Guid Id { get; set; }
        string Code { get; set; }
        int Percent {  get; set; }
        DateTime ValidFrom { get; set; }
        DateTime ValidTo { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        bool IsActive { get; set; }
    }
}
