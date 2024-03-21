using Autofac;
using HotelManager.Service.Common;


namespace HotelManager.Service
{
    public class ServiceModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<RoomService>().As<IRoomService>();
            builder.RegisterType<HotelServiceService>().As<IHotelServiceService>();
            builder.RegisterType<RoomTypeService>().As<IRoomTypeService>();
            builder.RegisterType<ReceiptService>().As<IReceiptService>();
            builder.RegisterType<ReviewService>().As<IReviewService>();
            builder.RegisterType<DiscountService>().As<IDiscountService>();
            builder.RegisterType<ReservationService>().As<IReservationService>();
        }
    }
}
