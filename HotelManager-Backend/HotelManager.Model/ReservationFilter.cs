using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Model
{
    public class ReservationFilter
    {
        public ReservationFilter() { }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public decimal? MinPricePerNight { get; set; }
        public decimal? MaxPricePerNight { get; set; }
        public string SearchQuery { get; set; }
        public Guid? UserId { get; set; }

    }
}
