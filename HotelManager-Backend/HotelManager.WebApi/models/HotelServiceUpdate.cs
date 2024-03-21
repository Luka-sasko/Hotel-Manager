using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManager.WebApi.Models
{
    public class HotelServiceUpdate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
    }
}