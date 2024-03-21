using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using HotelManager.Repository;
using HotelManager.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace HotelManager.WebApi.App_Start
{
    public class DIConfig
    {
        public static void Register(HttpConfiguration config)
        {
             var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule(new ServiceModules());
            builder.RegisterModule(new RepositoryModules());

            builder.AddAutoMapper(Assembly.GetExecutingAssembly());

            var contrainer = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(contrainer);
        }
       


    }
}