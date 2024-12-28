using Food.Web.Models;
using Food.Web.Services.IServices;
using static Food.Web.Utilities.StaticDetails;

namespace Food.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;
            public OrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }


        public async Task<ResponseDto> CreateOrder(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = OrderAPIBase + "/api/order/CreateOrder",
            });
        }

        public async Task<ResponseDto> CreateStripeSession(StripeRequestDto stripeRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = stripeRequestDto,
                Url = OrderAPIBase + "/api/order/CreateStripeSession",
            });
        }

        public async Task<ResponseDto?> GetAllOrders(string? userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = OrderAPIBase + "/api/order/GetOrders/" + userId,
            });
        }

        public async Task<ResponseDto?> GetOrder(int orderId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = OrderAPIBase + "/api/order/GetOrder/" + orderId,
            });
        }

        public async Task<ResponseDto?> UpdateOderStatus(int orderId, string newStatus)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = newStatus,
                Url = OrderAPIBase + "/api/order/UpdateOrderStatus/" + orderId,
            });
        }

        public async Task<ResponseDto> ValidateStripeSession(int orderHeaderId)

        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = orderHeaderId,
                Url = OrderAPIBase + "/api/order/ValidateStripeSession",
            });
        }
    }
}
