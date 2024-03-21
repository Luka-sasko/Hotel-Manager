using System;

namespace HotelManager.Common
{
    public class ReceiptFilter
    {
        public decimal minPrice { get; set; }
        public decimal? maxPrice { get; set; }
        public bool? isPaid { get; set; }
        public Guid? ReservationId { get; set; }
        public string userEmailQuery { get; set; }
        public DateTime? dateCreated { get; set; }
        public DateTime? dateUpdated { get; set; }
    }
}