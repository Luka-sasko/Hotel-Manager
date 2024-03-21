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
    public interface IReservationRepository
    {
        Task<IReservation> GetByIdAsync(Guid id);
        Task<PagedList<ReservationWithUserEmail>> GetAllAsync(Paging paging, Sorting sorting, ReservationFilter reservationFilter);

        Task<ReservationUpdate> UpdateAsync(Guid id, ReservationUpdate reservationUpdate, Guid userId);
        Task<Reservation> PostAsync(Reservation reservationCreate);
        Task<ReservationUpdate> DeleteAsync(Guid id);
        Task<bool> CheckIfAvailableAsync(ReservationRoom reservationRoom);
    }
}
