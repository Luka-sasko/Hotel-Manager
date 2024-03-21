using HotelManager.Common;
using HotelManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Service.Common
{
    public interface IReviewService
    {

        Task<PagedList<Review>> GetAllAsync(Paging paging, Sorting sorting, ReviewFilter reviewFilter);

        Task<bool> CreateAsync(Guid roomId, Review review);
    }
}
