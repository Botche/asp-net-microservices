namespace Discount.Grpc.Services
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Discount.Grpc.Entities;
    using Discount.Grpc.Protos;
    using Discount.Grpc.Repositories;

    using global::Grpc.Core;

    using static Discount.Grpc.Protos.DiscountProtoService;

    public class DiscountService : DiscountProtoServiceBase
    {
        private readonly IDiscountRepository discountRepository;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public DiscountService(IDiscountRepository discountRepository, ILogger logger, IMapper mapper)
        {
            this.discountRepository = discountRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, global::Grpc.Core.ServerCallContext context)
        {
            Coupon coupon = await this.discountRepository.GetDiscountAsync(request.ProductName);

            if (coupon == null)
            {
                string statusMessage = $"Discount with ProductName = {request.ProductName} is not found.";
                Status status = new(StatusCode.NotFound, statusMessage);
                throw new RpcException(status);
            }

            this.logger.LogInformation(
                "Discount is retrieved for ProductName: {productName}, Amount: {amount}", 
                coupon.ProductName, 
                coupon.Amount
            );

            CouponModel couponModel = this.mapper.Map<CouponModel>(coupon);

            return couponModel;
        }

        public override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, global::Grpc.Core.ServerCallContext context)
        {
            return base.CreateDiscount(request, context);
        }

        public override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, global::Grpc.Core.ServerCallContext context)
        {
            return base.UpdateDiscount(request, context);
        }

        public override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, global::Grpc.Core.ServerCallContext context)
        {
            return base.DeleteDiscount(request, context);
        }
    }
}
