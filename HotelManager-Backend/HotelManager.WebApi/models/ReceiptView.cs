using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManager.WebApi.Models
{
    public class ReceiptView
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsPaid { get; set; }
        public Guid ReservationId { get; set; }
        public DateTime DateCreated { get; set; }
        public string InvoiceNumber { get; set; }
        public string UserEmail { get; set; }
    }
}