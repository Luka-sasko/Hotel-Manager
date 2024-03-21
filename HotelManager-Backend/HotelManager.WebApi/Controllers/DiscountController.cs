using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Service;
using HotelManager.Service.Common;
using HotelManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HotelManager.WebApi.Controllers
{
    public class DiscountController : ApiController
    {
        private readonly IDiscountService _discountService;

        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }


        // GET: api/Discount
        [Authorize(Roles = "Admin, User")]
        public async Task<HttpResponseMessage> GetAsync(
            [FromUri] int startingValue = 0, 
            int endValue = 100, 
            string code="",
            int pageNumber = 1, 
            int pageSize = 10, 
            string sortOrder = "ASC", 
            string sortBy = "Percent", 
            DateTime? dateCreated = null, 
            DateTime? dateUpdated = null
            )
        {
            DiscountFilter filter = new DiscountFilter
            {
                StartingValue = startingValue,
                EndValue = endValue,
                Code = code
            };

            Sorting sorting = new Sorting
            {
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            Paging paging = new Paging
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            PagedList<IDiscount> discounts;
            PagedList<DiscountView> discountsView = new PagedList<DiscountView>();

            try
            {
                discounts = await _discountService.GetAllDiscountsAsync(filter, sorting, paging);

                discountsView.PageSize = discounts.PageSize;
                discountsView.PageNumber= discounts.PageNumber;
                discountsView.TotalCount = discounts.TotalCount;

                discountsView.Items = discounts.Items.Select(discount => new DiscountView
                {
                    Id = discount.Id,
                    Code = discount.Code,
                    Percent = discount.Percent,
                    ValidFrom = discount.ValidFrom,
                    ValidTo = discount.ValidTo
                }).ToList();

            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadGateway, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, discountsView);
        }

        // GET: api/Discount/5
        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> GetAsync(Guid id)
        {
            try
            {
                IDiscount discount = await _discountService.GetDiscountByIdAsync(id);
                DiscountView discountView = new DiscountView() { 
                    Id = discount.Id,
                    Code = discount.Code,
                    Percent = discount.Percent,
                    ValidFrom = discount.ValidFrom,
                    ValidTo = discount.ValidTo
                };
                return Request.CreateResponse(HttpStatusCode.OK, discountView);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadGateway, e.Message);
            }
        }

        // POST: api/Discount
        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> PostAsync([FromBody] Discount newDiscount)
        {

            IDiscount discount = new Discount
            {
                Id = Guid.NewGuid(),
                Code = newDiscount.Code,
                Percent = newDiscount.Percent,
                ValidFrom = newDiscount.ValidFrom,
                ValidTo = newDiscount.ValidTo,
                CreatedBy = newDiscount.CreatedBy,
                UpdatedBy = newDiscount.UpdatedBy,
                DateCreated = newDiscount.DateCreated,
                DateUpdated = newDiscount.DateUpdated,
                IsActive = newDiscount.IsActive
            };

            string result = await _discountService.CreateDiscountAsync(discount);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }


        // PUT: api/Discount/5
        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Put(Guid id, [FromBody] Discount discountUpdated)
        {
            string result = await _discountService.UpdateDiscountAsync(id, discountUpdated);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // DELETE: api/Discount/5
        [Authorize(Roles = "Admin")]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {

            int result = await _discountService.DeleteDiscountAsync(id);
            if (result == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Discount not found");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Discount deleted");
            }
        }
    }
}