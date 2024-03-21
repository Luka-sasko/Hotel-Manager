using HotelManager.Model.Common;
using System;

namespace HotelManager.WebApi.Models
{
    public class ServiceInvoiceHistory : IServiceInvoiceHistory
    {
        public Guid Id { get; set; }
        public string ServiceName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreated { get; set; }
    }
}