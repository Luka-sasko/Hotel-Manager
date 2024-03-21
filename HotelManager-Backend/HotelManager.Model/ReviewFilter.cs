using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Model
{
    public class ReviewFilter
    {
        public ReviewFilter() { }   
        public int Rating { get; set; }
        public string Comment { get; set; }
        public Guid RoomId { get; set; }
    }
}
