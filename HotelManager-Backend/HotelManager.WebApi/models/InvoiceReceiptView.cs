using HotelManager.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManager.WebApi.Models
{
    public class InvoiceReceiptView
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsPaid { get; set; }
        public Guid ReservationId { get; set; }
        public Guid? DiscountId { get; set; }
        public string InvoiceNumber { get; set; }
        public string ReservationNumber { get; set; }
        public decimal PricePerNight { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int RoomNumber { get; set; }
        public string Code { get; set; }
        public int Percent { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}