using Autofac;
using HotelManager.Model.Common;
using HotelManager.Repository.Common;


namespace HotelManager.Repository
{
    public class RepositoryModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        { 
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<RoomRepository>().As<IRoomRepository>();
            builder.RegisterType<HotelServiceRepository>().As<IHotelServiceRepository>();
            builder.RegisterType<RoomTypeRepository>().As<IRoomTypeRepository>();
            builder.RegisterType<ReceiptRepository>().As<IReceiptRepository>();
            builder.RegisterType<ServiceInvoiceRepository>().As<IServiceInvoiceRepository>();     
            builder.RegisterType<RoleTypeRepository>().As<IRoleTypeRepository>();
            builder.RegisterType<ReviewRepository>().As<IReviewRepository>();
            builder.RegisterType<DiscountRepository>().As<IDiscountRepository>();
            builder.RegisterType<ReservationRepository>().As<IReservationRepository>();
        }
    }
}
