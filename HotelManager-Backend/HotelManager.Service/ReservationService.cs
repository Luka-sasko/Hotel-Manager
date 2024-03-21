using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
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
    public class ReservationService : IReservationService

    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IReceiptService _receiptService;



        public ReservationService(IReceiptService receiptService,IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
            _receiptService = receiptService;

        }

        public async Task<PagedList<ReservationWithUserEmail>> GetAllAsync(Paging paging, Sorting sorting, ReservationFilter reservationFilter)
        {
            return await _reservationRepository.GetAllAsync(paging, sorting, reservationFilter);
        }

        public async Task<IReservation> GetByIdAsync(Guid id)
        {
            return await _reservationRepository.GetByIdAsync(id);
        }

        public async Task<Reservation> PostAsync(ReservationCreate reservationCreate)
        {
            if (await _reservationRepository.CheckIfAvailableAsync(new ReservationRoom { 
                RoomId = reservationCreate.RoomId, 
                CheckInDate = reservationCreate.CheckInDate, 
                CheckOutDate = reservationCreate.CheckOutDate 
            }))
            {
                Guid userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
                Guid reservationId = Guid.NewGuid();
                Guid invoiceId = Guid.NewGuid();


                string reservationNumber = GenerateReservationNumber();
                string receiptNumber = GenerateReceiptNumber();
                var numberOfDaysOfStay = CalculateNumberOfDays(reservationCreate.CheckInDate, reservationCreate.CheckOutDate);
                var totalPrice = numberOfDaysOfStay * reservationCreate.PricePerNight;

                Reservation reservation = new Reservation()
                {
                    UserId = userId,
                    Id = reservationId,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    ReservationNumber = reservationNumber,
                    RoomId = reservationCreate.RoomId,
                    PricePerNight = reservationCreate.PricePerNight,
                    CheckInDate = reservationCreate.CheckInDate,
                    CheckOutDate = reservationCreate.CheckOutDate,
                    IsActive = true
                };

                Invoice invoice = new Invoice()
                {
                    TotalPrice = totalPrice,
                    Id = invoiceId,
                    ReservationId = reservationId,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    InvoiceNumber = receiptNumber,
                    DiscountId = reservationCreate.DiscountId,
                    IsActive = true
                };
                Reservation reservationCreated = await _reservationRepository.PostAsync(reservation);
                Invoice invoiceCreated = await _receiptService.CreateInvoiceAsync(invoice);
                return reservationCreated;
            }
            return null;
        }

        public async Task<ReservationUpdate> UpdateAsync(Guid id, Guid invoiceId, ReservationUpdate reservationUpdate)
        {
            if (await _reservationRepository.CheckIfAvailableAsync(new ReservationRoom 
            {   
                RoomId = reservationUpdate.RoomId,
                CheckInDate = reservationUpdate.CheckInDate, 
                CheckOutDate = reservationUpdate.CheckOutDate
            }))
            {
                var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
                if (reservationUpdate.IsActive == true)
                {
                    var numberOfDaysOfStay = CalculateNumberOfDays(reservationUpdate.CheckInDate, reservationUpdate.CheckOutDate);
                    var totalPrice = numberOfDaysOfStay * reservationUpdate.PricePerNight;
                    InvoiceUpdate invoiceUpdate = new InvoiceUpdate()
                    {
                        IsActive = true,
                        UpdatedBy = userId,
                        TotalPrice = totalPrice
                    };

                    Invoice invoice = await _receiptService.PutTotalPriceAsync(invoiceId, invoiceUpdate);
                    return await _reservationRepository.UpdateAsync(id, reservationUpdate, userId);
                }
                else
                {
                    InvoiceUpdate invoiceUpdate = new InvoiceUpdate()
                    {
                        UpdatedBy = userId,
                        IsActive = false
                    };
                    Invoice invoice = await _receiptService.PutTotalPriceAsync(invoiceId, invoiceUpdate);
                    return await _reservationRepository.UpdateAsync(id, reservationUpdate, userId);

                }
            }
            return null;
        }
    

        static string GenerateReservationNumber()
        {
            string reservationPrefix = "RES";
            string currentDateStr = DateTime.Now.ToString("yyyyMMdd");
            string uniqueIdentifier = Guid.NewGuid().ToString().Substring(0, 8);

            return $"{reservationPrefix}{currentDateStr}-{uniqueIdentifier}";
        }
        static string GenerateReceiptNumber()
        {
            string receiptPrefix = "REC";
            string currentDateStr = DateTime.Now.ToString("yyyyMMdd");
            string uniqueIdentifier = Guid.NewGuid().ToString().Substring(0, 8);

            return $"{receiptPrefix}{currentDateStr}-{uniqueIdentifier}";
        }

        static int CalculateNumberOfDays(DateTime startDate, DateTime endDate)
        {
            startDate = startDate.Date;
            endDate = endDate.Date;
            TimeSpan timeSpan = endDate - startDate;
            return Math.Abs(timeSpan.Days);
        }

        public async Task<ReservationUpdate> DeleteAsync(Guid id, Guid invoiceId)
        {
            try
            {
                ReservationUpdate reservationUpdate = await _reservationRepository.DeleteAsync(id);

                if (reservationUpdate != null)
                {
                    InvoiceUpdate invoiceUpdate = new InvoiceUpdate
                    {
                        IsActive = false
                    };
                    await _receiptService.PutTotalPriceAsync(invoiceId, invoiceUpdate);
                    return reservationUpdate;
                }
                else
                {
                    throw new Exception("Reservation not found or could not be updated.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting reservation: " + ex.Message);
            }
        }

    }

}
