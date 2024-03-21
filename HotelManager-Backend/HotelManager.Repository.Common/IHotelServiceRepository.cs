using HotelManager.Common;
using HotelManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository.Common
{
    public interface IHotelServiceRepository
    {
        Task<PagedList<HotelService>> GetAllAsync(Paging paging, Sorting sorting, HotelServiceFilter hotelServiceFilter);
        Task<HotelService> GetByIdAsync(Guid id);

        Task<bool> CreateServiceAsync(HotelService hotelService);
        Task<bool> UpdateServiceAsync(Guid Id, HotelService service);
        Task<bool> DeleteServiceAsync(Guid Id, Guid userId);
    }
}
