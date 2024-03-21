using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Model
{
    public class ReservationCreate
    {
        public ReservationCreate() { }
        public Guid? DiscountId { get; set; }
        public Guid RoomId { get; set; }
        public decimal PricePerNight { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
