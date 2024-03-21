using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManager.WebApi.Models
{
    public class ReservationView
    {
        public Guid Id { get; set; }
        public string ReservationNumber { get; set; }
        public decimal PricePerNight { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string UserEmail { get; set; }
        public Guid RoomId { get; set; }
        public int RoomNumber { get; set; }
    }
}