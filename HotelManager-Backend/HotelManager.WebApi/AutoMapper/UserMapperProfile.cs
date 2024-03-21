using AutoMapper;
using HotelManager.Model;
using HotelManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelManager.WebApi.AutoMapper
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserView>();
            CreateMap<UserView, User>();
            CreateMap<UserRegistered, User>();
            CreateMap<UserUpdated, User>();
        }
    }
}