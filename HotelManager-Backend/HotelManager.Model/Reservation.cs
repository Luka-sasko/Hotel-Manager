using HotelManager.Model.Common;
using System;

namespace HotelManager.Model
{
    public class Reservation : IReservation
    {
        public Guid Id {get; set;} 
        public Guid UserId { get; set;}
        public Guid RoomId { get; set;}
        public string ReservationNumber { get; set; }
        public decimal PricePerNight { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set;}
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }
    }
}
