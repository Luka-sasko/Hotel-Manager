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
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace HotelManager.WebApi.Controllers
{
    [RoutePrefix("api/Review")]
    public class ReviewController : ApiController
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public ReviewController(IReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        // GET api/values
        [HttpGet]
        [Route("{roomId:guid}")]
        public async Task<HttpResponseMessage> GetReviewsForRoomAsync(int pageNumber = 1,
            int pageSize = 5,
            string sortBy = "",
            string isAsc = "ASC",
            string searchQuery = null,
            Guid roomId = default
            )
        {
            try
            {
                Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = isAsc };
                Paging paging = new Paging() { PageNumber = pageNumber, PageSize = pageSize };
                ReviewFilter reviewFilter = new ReviewFilter() { RoomId = roomId};
                var reviews = await _reviewService.GetAllAsync(paging, sorting, reviewFilter);

                if (reviews.Items.Any())
                {
                    var reviewViews = _mapper.Map<PagedList<ReviewView>>(reviews);
                    return Request.CreateResponse(HttpStatusCode.OK, reviewViews);
                }

                return Request.CreateResponse(HttpStatusCode.NotFound, "No reviews for the specified room!");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("{roomId:guid}")]
        public async Task<HttpResponseMessage> CreateReviewForRoomAsync(Guid roomId, [FromBody] Review review)
        {
            try
            {
                review.RoomId = roomId;
                review.DateCreated = DateTime.Now;
                review.DateUpdated = DateTime.Now;
                review.IsActive = true;

                await _reviewService.CreateAsync(roomId, review);

                return Request.CreateResponse(HttpStatusCode.Created, "Review successfully created.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //// PUT api/values/5
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}
    }
}
