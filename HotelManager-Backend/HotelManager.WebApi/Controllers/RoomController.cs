using AutoMapper;
using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Service.Common;
using HotelManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HotelManager.WebApi.Controllers
{
    public class RoomController : ApiController
    {
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;

        public RoomController(IRoomService roomService, IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "",
            string isAsc = "ASC",
            string searchQuery = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? minBeds = null,
            Guid? roomTypeId = null)
        {
            try { 
                Paging paging = new Paging() { PageNumber=pageNumber,PageSize=pageSize};
                Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = isAsc };
                RoomFilter roomFilter = new RoomFilter() { SearchQuery = searchQuery,StartDate=startDate,EndDate=endDate,MinBeds=minBeds,MaxPrice=maxPrice,MinPrice=minPrice,RoomTypeId=roomTypeId };
                var rooms = await _roomService.GetAllAsync(paging, sorting,roomFilter);
                
                if(rooms.Items.Any())
                {
                    var roomViews = _mapper.Map<PagedList<RoomView>>(rooms);
                    return Request.CreateResponse(HttpStatusCode.OK, roomViews);
                }
                return  Request.CreateResponse(HttpStatusCode.NotFound, "Room was not found!");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetByIdAsync(
            [FromUri] Guid id
            )
        {
            try
            {
                var room = await _roomService.GetByIdAsync(id);

                if (room != null)
                {


                    var roomView = _mapper.Map<RoomView>(room);
                    return Request.CreateResponse(HttpStatusCode.OK, roomView);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Room not found.");
                }
            }

            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
