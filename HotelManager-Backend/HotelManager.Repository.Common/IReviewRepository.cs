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
    public interface IReviewRepository
    {
        Task<PagedList<Review>> GetAllAsync(Paging paging, Sorting sorting, ReviewFilter reviewFilter);

        Task<bool> CreateAsync(Review review);
    }
}
