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
        private readonly ILogger<DiscountService> logger;
        private readonly IMapper mapper;

        public DiscountService(IDiscountRepository discountRepository, ILogger<DiscountService> logger, IMapper mapper)
        {
            this.discountRepository = discountRepository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
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

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            Coupon couponToCreate = this.mapper.Map<Coupon>(request.Coupon);
            await this.discountRepository.CreateDiscountAsync(couponToCreate);

            this.logger.LogInformation(
                "Discount is successfully created. ProductName: {productName}",
                couponToCreate.ProductName
            );

            CouponModel coupon = this.mapper.Map<CouponModel>(couponToCreate);

            return coupon;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            Coupon couponToUpdate = this.mapper.Map<Coupon>(request.Coupon);
            await this.discountRepository.UpdateDiscountAsync(couponToUpdate);

            this.logger.LogInformation(
                "Discount is successfully updated. ProductName: {productName}",
                couponToUpdate.ProductName
            );

            CouponModel coupon = this.mapper.Map<CouponModel>(couponToUpdate);

            return coupon;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            bool isDeleted = await this.discountRepository.DeleteDiscountAsync(request.ProductName);
            DeleteDiscountResponse response = new()
            {
                Success = isDeleted,
            };

            return response;
        }
    }
}
