namespace Discount.Grpc.Profiles
{
    using AutoMapper;

    using Discount.Grpc.Entities;
    using Discount.Grpc.Protos;

    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            this.CreateMap<Coupon, CouponModel>()
                .ReverseMap();
        }
    }
}
