using AutoMapper;
using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Service.Common;
using HotelManager.WebApi.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HotelManager.WebApi.Controllers
{
    [RoutePrefix("api/HotelService")]
    public class HotelServiceController : ApiController
    {
        private readonly IHotelServiceService _hotelServiceService;
        private readonly IMapper _mapper;

        public HotelServiceController(IHotelServiceService hotelServiceService, IMapper mapper)
        {
            _hotelServiceService = hotelServiceService;
            _mapper = mapper;
        }

        // GET api/values
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("")]
        public async Task<HttpResponseMessage> GetAllServicesAsync(
            [FromUri] int pageNumber = 1,
            [FromUri] int pageSize = 10,
            [FromUri] string sortBy = "",
            [FromUri] string isAsc = "ASC",
            [FromUri] string searchQuery = null,
            [FromUri] decimal? minPrice = null,
            [FromUri] decimal? maxPrice = null
            )
        {
            try
            {
                Paging paging = new Paging() { PageNumber = pageNumber, PageSize = pageSize };
                Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = isAsc.ToUpper() };
                HotelServiceFilter hotelServiceFilter = new HotelServiceFilter() { SearchQuery = searchQuery, MinPrice = minPrice, MaxPrice = maxPrice };

                var services = await _hotelServiceService.GetAllAsync(paging, sorting, hotelServiceFilter);
                if (services.Items.Any())
                {
                    var serviceViews = services.Items.Select(s => _mapper.Map<HotelServiceView>(s)).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, services);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound, "No services found.");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

        // GET api/values/5
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<HttpResponseMessage> GetServiceByIdAsync([FromUri] Guid id)
        {
            try
            {
                if (id.Equals(Guid.Empty))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid ID provided.");
                }

                var service = await _hotelServiceService.GetByIdAsync(id);
                if (service == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No hotel service with such ID found.");
                }

                var serviceView = _mapper.Map<HotelServiceView>(service);

                return Request.CreateResponse(HttpStatusCode.OK, serviceView);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // POST api/values
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> CreateServiceAsync([FromBody] HotelServiceCreate hotelServiceCreate)
        {
            if (hotelServiceCreate == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid data");
            }

            try
            {
                var hotelService = _mapper.Map<HotelService>(hotelServiceCreate);

                bool created = await _hotelServiceService.CreateServiceAsync(hotelService);
                if (created)
                {
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Could not create the service");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        // PUT api/values/5
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<HttpResponseMessage> UpdateServiceAsync(Guid id, [FromBody] HotelServiceUpdate hotelServiceUpdated)
        {
            if(hotelServiceUpdated == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                // Check if the service with the given id exists
                var serviceInBase = await _hotelServiceService.GetByIdAsync(id);
                if (serviceInBase == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Service not found");
                }

                // Map the update model to the domain model
                var hotelService = _mapper.Map<HotelService>(hotelServiceUpdated);

                // Update the service in the service layer
                bool updated = await _hotelServiceService.UpdateServiceAsync(id, hotelService);

                if (updated)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Failed to update service");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex);
            }
        }

        // DELETE api/values/5
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<HttpResponseMessage> DeleteService(Guid id)
        {
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            Task<HotelService> service = _hotelServiceService.GetByIdAsync(id);
            if (service == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {
                bool deleted = await _hotelServiceService.DeleteServiceAsync(id);
                if (deleted)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(InternalServerError(ex));
            }
        }
    }
}
