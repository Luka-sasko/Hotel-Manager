using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Service.Common;
using HotelManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace HotelManager.WebApi.Controllers
{
    public class DashBoardRoomController : ApiController
    {

        private readonly IRoomService _roomService;

        public DashBoardRoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<HttpResponseMessage> UpdateRoomAsync(
            [FromUri] Guid id
           , [FromBody] RoomUpdate roomUpdate
           )
        {
            try
            {
                if (id.Equals(Guid.Empty))
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                return Request.CreateResponse(HttpStatusCode.OK, await _roomService.UpdateRoomAsync(id, roomUpdate));

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllAsync(
            [FromUri] int pageNumber = 1,
            [FromUri] int pageSize = 10,
            [FromUri] string sortBy = "",
            [FromUri] string isAsc = "ASC",
            [FromUri] string searchQuery = null,
            [FromUri] DateTime? startDate = null,
            [FromUri] DateTime? endDate = null,
            [FromUri] decimal? minPrice = null,
            [FromUri] decimal? maxPrice = null,
            [FromUri] int? minBeds = null,
            [FromUri] Guid? roomTypeId = null)
        {
            try
            {
                Paging paging = new Paging() { PageNumber = pageNumber, PageSize = pageSize };
                Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = isAsc };
                RoomFilter roomFilter = new RoomFilter() { SearchQuery = searchQuery, StartDate = startDate, EndDate = endDate, MinBeds = minBeds, MaxPrice = maxPrice, MinPrice = minPrice, RoomTypeId = roomTypeId };
                var roomsUpdated = await _roomService.GetUpdatedRoomsAsync(paging, sorting, roomFilter);
                if (roomsUpdated.Items.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, roomsUpdated);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "Room was not found!");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> GetRoomsUpdateByIdAsync(
            [FromUri] Guid id
            )
        {
            try
            {
                if (id.Equals(Guid.Empty))
                    return Request.CreateResponse(HttpStatusCode.BadRequest);


                return Request.CreateResponse(HttpStatusCode.OK, await _roomService.GetRoomUpdateByIdAsync(id));
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPost] 
        public async Task<HttpResponseMessage> PostRoomAsync([FromBody] RoomCreate roomCreate)
        {
            try
            {
                if (roomCreate == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid room data");

                Guid typeId;
                if (!Guid.TryParse(roomCreate.TypeId, out typeId))
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid TypeId format");

                var room = new Room
                {
                    Number = roomCreate.Number,
                    BedCount = roomCreate.BedCount,
                    Price = roomCreate.Price,
                    IsActive= roomCreate.IsActive,
                    IsAvailable = roomCreate.IsAvailable ?? false,
                    ImageUrl = roomCreate.ImageUrl,
                    TypeId = typeId
                };

                var createdRoom = await _roomService.PostRoomAsync(room);
                return Request.CreateResponse(HttpStatusCode.Created, createdRoom);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
