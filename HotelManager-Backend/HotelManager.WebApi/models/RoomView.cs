using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManager.WebApi.Models
{
    public class RoomView
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public int BedCount { get; set; }
        public decimal Price { get; set; }
        public bool? IsAvailable { get; set; }
        public string ImageUrl { get; set; }
        public string TypeName { get; set; }
    }
}