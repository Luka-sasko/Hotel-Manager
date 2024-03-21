using AutoMapper;
using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Service.Common;
using HotelManager.WebApi.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HotelManager.WebApi.Controllers
{
    public class RoomTypeController : ApiController
    {

        private readonly IRoomTypeService _roomTypeService;
        private readonly IMapper _mapper;
 
        public RoomTypeController(IRoomTypeService roomTypeService, IMapper mapper)
        {
            _roomTypeService = roomTypeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAllRoomTypesAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "",
            string sortOrder = "ASC"
            )
        {
            try
            {
                Paging paging = new Paging() { PageNumber = pageNumber, PageSize = pageSize };
                Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = sortOrder };
                var roomTypes = await _roomTypeService.GetAllAsync(paging, sorting);

                if (roomTypes.Items.Any())
                {
                    var roomTypeViews = _mapper.Map<PagedList<RoomTypeView>>(roomTypes);
                    return Request.CreateResponse(HttpStatusCode.OK, roomTypeViews);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound, "Room type was not found!");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetRoomTypeById(
                [FromUri] Guid id)
        {
            try
            {
                var roomType = await _roomTypeService.GetByIdAsync(id);

                if (roomType != null && roomType.Id != Guid.Empty)
                {
                    var roomTypeView = _mapper.Map<RoomTypeView>(roomType);
                    return Request.CreateResponse(HttpStatusCode.OK, roomTypeView);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound, "Room type was not found!");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateRoomTypeAsync([FromBody] RoomTypePost roomTypePost)
        {
            try
            {
                var roomType = _mapper.Map<RoomType>(roomTypePost);
                var createdRoomType = await _roomTypeService.PostAsync(roomType);
                var roomTypeView = _mapper.Map<RoomTypeView>(createdRoomType);
                return Request.CreateResponse(HttpStatusCode.OK, roomTypeView);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateRoomTypeAsync([FromUri] Guid id, [FromBody] RoomTypeUpdate roomTypeUpdate)
        {
            try
            {
                var updatedRoomType = await _roomTypeService.UpdateAsync(id, roomTypeUpdate);
                return Request.CreateResponse(HttpStatusCode.OK, updatedRoomType);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteRoomTypeAsync([FromUri] Guid id)
        {
            try
            {
                bool result = await _roomTypeService.DeleteAsync(id);
                if (result)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Room type successfully deleted.");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Room type not found or already deleted.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}