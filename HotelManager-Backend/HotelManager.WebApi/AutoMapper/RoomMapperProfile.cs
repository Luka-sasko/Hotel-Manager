using AutoMapper;
using HotelManager.Common;
using HotelManager.Model;
using HotelManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManager.WebApi.AutoMapper
{
    public class RoomMapperProfile : Profile
    {
        public RoomMapperProfile()
        {
            CreateMap<Room, RoomView>();
            CreateMap<PagedList<Room>, PagedList<RoomView>>();
        }
    }
}