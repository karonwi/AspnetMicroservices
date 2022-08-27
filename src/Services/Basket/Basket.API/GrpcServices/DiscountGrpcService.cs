using Discount.Grpc.Protos;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService;
        }

        //we make gRPC calls with the GetDiscount
        public async Task<CouponModel> GetDiscount(string productName)
        {
            //making a request to the gRPC server
            var discountRequest = new GetDiscountRequest { ProductName = productName };
            return await _discountProtoService.GetDiscountAsync(discountRequest);
        }

        //returns a couponModel because that is what it wants to get as a request
        //and the gRPC server has already helped out to do the logic of getting that response
        public async Task<CouponModel> CreateDiscount(CreateDiscountRequest createDiscount)
        {
            var createDiscountRequest = new CreateDiscountRequest { Coupon = createDiscount.Coupon };
            return await _discountProtoService.CreateDiscountAsync(createDiscountRequest);
        }

    }
}
