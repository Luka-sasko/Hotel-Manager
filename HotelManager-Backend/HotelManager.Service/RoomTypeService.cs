using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace HotelManager.Service
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository RoomTypeRepository;

        public RoomTypeService(IRoomTypeRepository roomTypeRepository)
        {
            RoomTypeRepository = roomTypeRepository;
        }

        public async Task<PagedList<RoomType>> GetAllAsync(Paging paging, Sorting sorting)
        {
            return await RoomTypeRepository.GetAllAsync(paging,sorting);
        }

        public async Task<RoomType> GetByIdAsync(Guid id)
        {
            return await RoomTypeRepository.GetByIdAsync(id);
        }

        public async Task<RoomType> PostAsync(RoomType roomType)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            roomType.CreatedBy = userId;
            roomType.UpdatedBy = userId;
            roomType.DateCreated = DateTime.UtcNow;
            roomType.DateUpdated = DateTime.UtcNow;
            return await RoomTypeRepository.PostAsync(roomType, userId);
        }

        public  async Task<RoomTypeUpdate> UpdateAsync(Guid id, RoomTypeUpdate roomTypeUpdate)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            roomTypeUpdate.DateUpdated = DateTime.UtcNow;
            roomTypeUpdate.UpdatedBy = userId;
            return await RoomTypeRepository.UpdateAsync(id, roomTypeUpdate, userId);    
        }
       
        public async Task<bool> DeleteAsync(Guid id) { 
            return await RoomTypeRepository.DeleteAsync(id);
        }
    }
}
