using HotelManager.Model.Common;
using System;

namespace HotelManager.Model
{
    public class Discount : IDiscount
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public int Percent {  get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool IsActive { get; set; }
    }
}
