using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Model
{
    public class Room
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public int BedCount { get; set; }
        public decimal Price { get; set; }
        public bool? IsAvailable { get; set; }
        public string ImageUrl { get; set; }
        public Guid TypeId { get; set; }
        public string TypeName { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }

        public Room()
        {
            
        }
    }
}
