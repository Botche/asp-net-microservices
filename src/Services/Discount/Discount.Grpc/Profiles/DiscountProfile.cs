namespace Discount.Grpc.Profiles
{
    using AutoMapper;

    using Discount.Data.Entities;
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
