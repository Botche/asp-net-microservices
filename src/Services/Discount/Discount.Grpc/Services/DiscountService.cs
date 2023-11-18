namespace Discount.Grpc.Services
{
    using System.Threading.Tasks;

    using Discount.Grpc.Entities;
    using Discount.Grpc.Protos;
    using Discount.Grpc.Repositories;

    using global::Grpc.Core;

    using static Discount.Grpc.Protos.DiscountProtoService;

    public class DiscountService : DiscountProtoServiceBase
    {
        private readonly IDiscountRepository discountRepository;
        private readonly ILogger logger;

        public DiscountService(IDiscountRepository discountRepository, ILogger logger)
        {
            this.discountRepository = discountRepository;
            this.logger = logger;
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

            CouponModel couponModel = mapper.Map<CouponModel>(coupon);

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
