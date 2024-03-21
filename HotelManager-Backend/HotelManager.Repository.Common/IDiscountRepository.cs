using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository.Common
{
    public interface IDiscountRepository
    {
        Task<PagedList<IDiscount>> GetAllDiscountsAsync(DiscountFilter filter, Sorting sorting, Paging paging);
        Task<IDiscount> GetDiscountByIdAsync(Guid id);
        Task<int> DeleteDiscountActiveAsync(Guid id);
        Task<string> CreateDiscountAsync(IDiscount newDiscount, Guid userId);
        Task<string> UpdateDiscountAsync(Guid id ,IDiscount discountUpdated, Guid userId);
    }
}
