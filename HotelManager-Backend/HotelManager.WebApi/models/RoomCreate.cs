using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManager.WebApi.Models
{
    public class RoomCreate
    {
        public int Number { get; set; }
        public int BedCount { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;

        public bool? IsAvailable { get; set; }
        public string ImageUrl { get; set; }
        public string TypeId { get; set; }
    }
}