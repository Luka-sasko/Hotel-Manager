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
    public class HotelServiceService : IHotelServiceService
    {
        private readonly IHotelServiceRepository _HotelServiceRepository;

        public HotelServiceService(IHotelServiceRepository hotelServiceRepository)
        {
            _HotelServiceRepository = hotelServiceRepository;
        }

        public async Task<PagedList<HotelService>> GetAllAsync(Paging paging, Sorting sorting, HotelServiceFilter hotelServiceFilter)
        {
            var sericesPagedList = await _HotelServiceRepository.GetAllAsync(paging, sorting, hotelServiceFilter);
            if(sericesPagedList == null)
            {
                throw new ArgumentException("No services found!");
            }
            return sericesPagedList;
        }

        public async Task<HotelService> GetByIdAsync(Guid id)
        {
            var service = await _HotelServiceRepository.GetByIdAsync(id);
            if(service == null)
            {
                return null;
            }
            return service;
        }

        public Task<bool> CreateServiceAsync(HotelService hotelService)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
                hotelService.DateCreated = DateTime.UtcNow;
                hotelService.DateUpdated = DateTime.UtcNow;
                hotelService.CreatedBy = userId;
                hotelService.UpdatedBy = userId;
                hotelService.IsActive = true;
                return _HotelServiceRepository.CreateServiceAsync(hotelService);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> UpdateServiceAsync(Guid Id, HotelService hotelService)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
                hotelService.UpdatedBy = userId;
                hotelService.DateUpdated = DateTime.UtcNow;
                return _HotelServiceRepository.UpdateServiceAsync(Id, hotelService);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public Task<bool> DeleteServiceAsync(Guid Id)
        {
            try
            {
                var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
                return _HotelServiceRepository.DeleteServiceAsync(Id, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
