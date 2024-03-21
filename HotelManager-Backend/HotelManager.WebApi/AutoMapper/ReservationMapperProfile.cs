using AutoMapper;
using HotelManager.Common;
using HotelManager.Model;
using HotelManager.WebApi.Models;

namespace HotelManager.WebApi.AutoMapper
{
    public class ReservationMapperProfile : Profile
    {
        public ReservationMapperProfile() {
            CreateMap<ReservationWithUserEmail, ReservationView>();
            CreateMap<PagedList<ReservationWithUserEmail>, PagedList<ReservationView>>();
            CreateMap<Reservation, ReservationEditView>();
        }
    }
}