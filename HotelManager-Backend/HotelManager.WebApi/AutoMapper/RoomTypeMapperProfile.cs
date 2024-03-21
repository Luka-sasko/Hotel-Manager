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
    public class RoomTypeMapperProfile : Profile
    {
        public RoomTypeMapperProfile()
        {
            CreateMap<RoomTypePost, RoomType>();
            CreateMap<RoomTypeUpdate, RoomType>();
            CreateMap<RoomType, RoomTypeView>();
            CreateMap<PagedList<RoomType>, PagedList<RoomTypeView>>();
        }
    }
}