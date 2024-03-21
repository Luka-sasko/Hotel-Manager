using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Service.Common;
using HotelManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Routing;

namespace HotelManager.WebApi.Controllers
{
    public class ServiceInvoiceController : ApiController
    {
        private readonly IReceiptService _receiptService;

        public ServiceInvoiceController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetServiceInvoiceByInvoiceIdAsync (
            Guid id,
            string sortBy = "DateCreated",
            string isAsc = "ASC")
        {
            Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = isAsc };
            try
            {
                PagedList<IServiceInvoiceHistory> serviceInvoiceHistories = await _receiptService.GetServiceInvoiceByInvoiceIdAsync(id,sorting);
                return Request.CreateResponse(HttpStatusCode.OK, serviceInvoiceHistories);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadGateway, e.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllInvoiceServiceAsync(int pageNumber = 1, int pageSize = 10, string sortOrder = "ASC", string sortBy = "DateCreated", DateTime? dateCreated = null, DateTime? dateUpdated = null)
        {
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

            PagedList<IServiceInvoice> invoices;

            try
            {
                invoices = await _receiptService.GetAllInvoiceServiceAsync(sorting, paging);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadGateway, e.Message);
            }

            return Request.CreateResponse(HttpStatusCode.OK, invoices);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            int result = await _receiptService.DeleteAsync(id);
            if (result == 0)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Invoice not found");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Invoice deleted");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<HttpResponseMessage> CreateInvoiceServiceAsync([FromBody] ServiceInvoiceCreate invoiceCreate)
        {
            IServiceInvoice invoiceAdd = new ServiceInvoice
            {
                NumberOfService = invoiceCreate.Quantity,
                InvoiceId = invoiceCreate.InvoiceId,
                ServiceId = invoiceCreate.ServiceId,
            };

            string result = await _receiptService.CreateInvoiceServiceAsync(invoiceAdd);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}
