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
    public interface IRoomService
    {
        Task<Room> GetByIdAsync(Guid id);
        Task<PagedList<Room>> GetAllAsync(Paging paging, Sorting sorting, RoomFilter roomFilter);
        Task<RoomUpdate> UpdateRoomAsync(Guid id, RoomUpdate roomUpdate);
        Task<RoomUpdate> GetRoomUpdateByIdAsync(Guid id);
        Task<PagedList<RoomUpdate>> GetUpdatedRoomsAsync(Paging paging, Sorting sorting, RoomFilter roomsFilter);
        Task<Room> PostRoomAsync(Room room);

    }
}
