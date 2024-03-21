using AutoMapper;
using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManager.WebApi.AutoMapper
{
    public class ReceiptMapperProfile : Profile
    {
        public ReceiptMapperProfile()
        {
            CreateMap<Receipt, ReceiptView>();
            CreateMap<PagedList<Receipt>, PagedList<ReceiptView>>();
            CreateMap<InvoiceReceipt, InvoiceReceiptView>()
                .ForMember(dest => dest.ReservationNumber, opt => opt.MapFrom(src => src.Reservation.ReservationNumber))
                .ForMember(dest => dest.PricePerNight, opt => opt.MapFrom(src => src.Reservation.PricePerNight))
                .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => src.Reservation.CheckInDate))
                .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => src.Reservation.CheckOutDate))
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.RoomNumber))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Discount.Code))
                .ForMember(dest => dest.Percent, opt => opt.MapFrom(src => src.Discount.Percent))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));
        }
    }
}