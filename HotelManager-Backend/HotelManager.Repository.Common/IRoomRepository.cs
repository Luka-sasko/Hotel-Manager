using HotelManager.Common;
using HotelManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository.Common
{

    public interface IRoomRepository
    {
        Task<Room> GetByIdAsync(Guid id);
        Task<PagedList<Room>> GetAllAsync(Paging paging, Sorting sorting, RoomFilter roomFilter);

        Task<Room> PostRoomAsync(Room room);
        Task <RoomUpdate> UpdateRoomAsync(Guid id,RoomUpdate roomUpdate, Guid userId);
        Task <RoomUpdate> GetRoomUpdateByIdAsync(Guid id);
        Task<PagedList<RoomUpdate>> GetUpdatedRoomsAsync(Paging paging, Sorting sorting, RoomFilter roomsFilter);
    }
}

