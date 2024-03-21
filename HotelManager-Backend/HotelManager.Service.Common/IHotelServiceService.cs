using HotelManager.Common;
using HotelManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Service.Common
{
    public interface IHotelServiceService
    {
        Task<PagedList<HotelService>> GetAllAsync(Paging paging, Sorting sorting, HotelServiceFilter hotelServiceFilter);
        Task<HotelService> GetByIdAsync(Guid id);
        Task<bool> CreateServiceAsync(HotelService hotelService);
        Task<bool> UpdateServiceAsync(Guid Id,HotelService hotelService);
        Task<bool> DeleteServiceAsync(Guid Id);
    }
}