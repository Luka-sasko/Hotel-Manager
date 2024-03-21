using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Model
{
    public class RoomFilter
    {
        public string SearchQuery { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinBeds { get; set; }
        public Guid? RoomTypeId { get; set; }

        public RoomFilter()
        {
            MinBeds = 1;
            MinPrice = 0;
        }
    }

}
