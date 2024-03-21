using System;

namespace HotelManager.Model.Common
{
    public interface IInvoiceReceipt
    {
        Guid Id { get; set; }
        decimal TotalPrice { get; set; }
        bool IsPaid { get; set; }
        Guid ReservationId { get; set; }
        Guid? DiscountId { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DateUpdated { get; set; }
        bool IsActive { get; set; }
        string InvoiceNumber { get; set; }
        IReservation Reservation { get; set; }
        int RoomNumber { get; set; }
        IDiscount Discount { get; set; }
        IUser User { get; set; }
    }
}
