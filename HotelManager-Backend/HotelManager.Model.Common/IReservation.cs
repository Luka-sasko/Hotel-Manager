using System;

namespace HotelManager.Model.Common
{
    public interface IReservation
    {
        Guid Id { get; set; }
        Guid UserId { get; set; }
        Guid RoomId { get; set; }
        string ReservationNumber { get; set; }
        decimal PricePerNight { get; set; }
        DateTime CheckInDate { get; set; }
        DateTime CheckOutDate { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime DateUpdated { get; set; }
        DateTime DateCreated { get; set; }
        bool IsActive { get; set; }
    }
}
