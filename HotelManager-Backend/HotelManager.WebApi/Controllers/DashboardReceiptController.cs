using AutoMapper;
using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Service.Common;
using HotelManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace HotelManager.WebApi.Controllers
{
    public class DashboardReceiptController : ApiController
    {

        private readonly IReceiptService _receiptService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public DashboardReceiptController(IReceiptService receiptService, IUserService profileService, IMapper mapper)
        {
            _receiptService = receiptService;
            _userService = profileService;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        // GET: api/DashboardReceipt
        public async Task<HttpResponseMessage> GetReceipts([FromUri] decimal minPrice = 0, decimal? maxPrice = null, bool? isPaid = null, string userEmailQuery = null, int pageNum = 1, int pageSize = 10, string sortOrder = "ASC", string sortBy = "TotalPrice", DateTime? dateCreated = null, DateTime? dateUpdated = null, Guid? reservationId = null)
        {
            ReceiptFilter filter = new ReceiptFilter
            {
                minPrice = minPrice,
                maxPrice = maxPrice,
                userEmailQuery = userEmailQuery,
                dateCreated = dateCreated,
                dateUpdated = dateUpdated,
                isPaid = isPaid,
                ReservationId = reservationId
            };

            Sorting sorting = new Sorting
            {
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            Paging paging = new Paging
            {
                PageNumber = pageNum,
                PageSize = pageSize
            };

            PagedList<IReceipt> receipts;

            try
            {
                receipts = await _receiptService.GetAllAsync(filter, sorting, paging);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadGateway, e.Message);
            }
 
            PagedList<ReceiptView> receiptViews = new PagedList<ReceiptView>();
            receiptViews.Items = new  List<ReceiptView>();
            foreach (var receipt in receipts.Items)
            {
                var receiptView = _mapper.Map<ReceiptView>(receipt);
                receiptView.UserEmail = await _userService.GetUserEmailByIdAsync(receipt.CreatedBy);
                receiptViews.Items.Add(receiptView);
            }
            receiptViews.TotalCount = receipts.TotalCount;
            receiptViews.PageSize = receipts.PageSize;
            receiptViews.PageNumber = receipts.PageNumber;

            return Request.CreateResponse(HttpStatusCode.OK, receiptViews);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        // GET: api/DashboardReceipt/5
        public async Task<HttpResponseMessage> GetReceipt(Guid id)
        {
            try
            {
                IInvoiceReceipt receipt = await _receiptService.GetByIdAsync(id);
                var receiptView = _mapper.Map<InvoiceReceiptView>(receipt);
                return Request.CreateResponse(HttpStatusCode.OK, receiptView);
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadGateway, e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        // POST: api/DashboardReceipt
        public async Task<HttpResponseMessage> SendReceipt(Guid id)
        {
            try
            {
                bool isSent = await _receiptService.SendReceiptAsync(id);
                if (isSent)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Receipt sent successfully");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Receipt could not be sent");
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadGateway, e.Message);
            }
        }
        
    }
}