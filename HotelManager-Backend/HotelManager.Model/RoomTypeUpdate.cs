using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Model
{
    public class RoomTypeUpdate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }

        public RoomTypeUpdate() { }
    }
}
