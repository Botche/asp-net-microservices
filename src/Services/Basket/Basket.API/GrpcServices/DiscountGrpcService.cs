namespace Basket.API.GrpcServices
{
    using Discount.Grpc.Protos;

    using static Discount.Grpc.Protos.DiscountProtoService;

    public class DiscountGrpcService
    {
        private readonly DiscountProtoServiceClient discountProtoService;

        public DiscountGrpcService(DiscountProtoServiceClient discountProtoService)
        {
            this.discountProtoService = discountProtoService;
        }

        public async Task<CouponModel> GetDiscountAsync(string productModel)
        {
            GetDiscountRequest discountRequest = new()
            {
                ProductName = productModel,
            };

            return await this.discountProtoService.GetDiscountAsync(discountRequest);
        }
    }
}
