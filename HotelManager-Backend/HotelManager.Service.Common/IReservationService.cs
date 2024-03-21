using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Service.Common
{
    public interface IReservationService
    {

        Task<IReservation> GetByIdAsync(Guid id);
        Task<PagedList<ReservationWithUserEmail>> GetAllAsync(Paging paging, Sorting sorting, ReservationFilter reservationFilter);

        Task<ReservationUpdate> UpdateAsync(Guid id, Guid invoiceId,ReservationUpdate reservationUpdate);
        Task<Reservation> PostAsync(ReservationCreate reservation);
        Task<ReservationUpdate> DeleteAsync(Guid id, Guid invoiceId);
    }
}
