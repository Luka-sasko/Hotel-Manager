using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HotelManager.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IRoomRepository _roomRepository;


        public ReviewService (IReviewRepository reviewRepository, IRoomRepository roomRepository)
        {
            _reviewRepository = reviewRepository;
            _roomRepository = roomRepository;
        }

        public async Task<PagedList<Review>> GetAllAsync(Paging paging, Sorting sorting, ReviewFilter reviewFilter)
        {
            try
            {
                return await _reviewRepository.GetAllAsync(paging, sorting, reviewFilter);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CreateAsync(Guid roomId, Review review)
        {
            if (review.Rating < 1 || review.Rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5.");
            }

            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            var roomExists = await _roomRepository.GetByIdAsync(roomId);
            Guid reviewId = Guid.NewGuid();

            if (roomExists == null)
            {
                throw new Exception("Room with the specified Id does not exist.");
            }

            try
            {
                review.RoomId = roomId;
                review.Id = reviewId;
                review.CreatedBy = userId;
                review.UpdatedBy = userId;
                review.DateCreated = DateTime.UtcNow;
                review.DateUpdated = DateTime.UtcNow;
                review.IsActive = true;
                return await _reviewRepository.CreateAsync(review);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
