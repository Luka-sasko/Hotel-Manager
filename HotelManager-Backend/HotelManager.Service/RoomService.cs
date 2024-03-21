using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HotelManager.Service
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository RoomRepository;

        public RoomService(IRoomRepository roomRepository)
        {
            RoomRepository = roomRepository;
        }
        public async Task<PagedList<Room>> GetAllAsync(Paging paging, Sorting sorting, RoomFilter roomFilter)
  
        {
            var rooms = await RoomRepository.GetAllAsync(paging, sorting, roomFilter);
            if(rooms==null)
                throw new ArgumentException("Rooms not found");
            return rooms;
        }

        public async Task<Room> GetByIdAsync(Guid id)
        {
            var room= await RoomRepository.GetByIdAsync(id);
            if(room == null)
                throw new ArgumentException("Room not found");
            return room;
        }

        public async Task<RoomUpdate> GetRoomUpdateByIdAsync(Guid id)
        {
            var roomUpdated = await RoomRepository.GetRoomUpdateByIdAsync(id);
           if(roomUpdated == null)
                return null;
            return roomUpdated;

        }

        public async Task<PagedList<RoomUpdate>> GetUpdatedRoomsAsync(Paging paging, Sorting sorting, RoomFilter roomsFilter)
        {
            var roomsUpdated = await RoomRepository.GetUpdatedRoomsAsync(paging, sorting, roomsFilter);
            if (roomsUpdated == null)
                return null;
            return roomsUpdated;
            
        }

        public async Task<Room> PostRoomAsync(Room room)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());

            room.Id = Guid.NewGuid();
            room.CreatedBy = userId;
            room.UpdatedBy= userId;
            room.DateUpdated = DateTime.UtcNow;
            room.DateCreated = DateTime.UtcNow;


            return await RoomRepository.PostRoomAsync(room);
        }


        public async Task<RoomUpdate> UpdateRoomAsync(Guid id, RoomUpdate roomUpdate)
        {
            var room = await RoomRepository.GetByIdAsync(id);
            if(room == null)
                throw new ArgumentException("Room not found");

            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            roomUpdate.DateUpdated = DateTime.UtcNow;
            roomUpdate.UpdatedBy = userId;
            return await RoomRepository.UpdateRoomAsync(id,roomUpdate, userId);
        }
    }
}
