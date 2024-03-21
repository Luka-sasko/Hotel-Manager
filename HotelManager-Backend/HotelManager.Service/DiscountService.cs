using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Repository;
using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HotelManager.Service
{
    public class DiscountService : IDiscountService
    {
        private readonly IDiscountRepository _discountRepository;

        public DiscountService(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<string> CreateDiscountAsync(IDiscount newDiscount)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            newDiscount.CreatedBy = userId;
            newDiscount.UpdatedBy = userId;
            newDiscount.DateUpdated = DateTime.UtcNow;
            newDiscount.DateCreated = DateTime.UtcNow;
            try
            {
                return await _discountRepository.CreateDiscountAsync(newDiscount, userId);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<int> DeleteDiscountAsync(Guid id)
        {
            try
            {
                return await _discountRepository.DeleteDiscountActiveAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PagedList<IDiscount>> GetAllDiscountsAsync(DiscountFilter filter, Sorting sorting, Paging paging)
        {
            try
            {
                return await _discountRepository.GetAllDiscountsAsync(filter, sorting, paging);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IDiscount> GetDiscountByIdAsync(Guid id)
        {
            try
            {
                return await _discountRepository.GetDiscountByIdAsync(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        

        public async Task<string> UpdateDiscountAsync(Guid id, IDiscount discountUpdated)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            discountUpdated.UpdatedBy = userId;
            discountUpdated.DateUpdated = DateTime.UtcNow;
            try
            {
                return await _discountRepository.UpdateDiscountAsync(id, discountUpdated, userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
