using HotelManager.Common;
using HotelManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository.Common
{
    public interface IRoomTypeRepository
    {
        Task<RoomType> GetByIdAsync(Guid id);
        Task<PagedList<RoomType>> GetAllAsync(Paging paging, Sorting sorting);
        Task<RoomTypeUpdate> UpdateAsync(Guid id, RoomTypeUpdate roomTypeUpdate, Guid roomId);
        Task<RoomType> PostAsync(RoomType roomType, Guid roomId);
        Task<bool> DeleteAsync(Guid id);
    }
}
