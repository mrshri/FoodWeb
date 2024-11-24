using Food.Web.Models;
using Food.Web.Services.IServices;
using Food.Web.Utilities;

namespace Food.Web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IBaseService _baseService;

        public ShoppingCartService(IBaseService baseService)
        {
            _baseService = baseService;

        }
        public async Task<ResponseDto> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = cartDto,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/ApplyCoupon",

            });
        }

        public async Task<ResponseDto> GetCartByUserIDAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/GetCart/" + userId,
            });
        }

        public async Task<ResponseDto> RemoveFromCartAsync(int CartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = CartDetailsId,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/RemoveCart",

            });
        }

        public async Task<ResponseDto> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = cartDto,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/CartUpsert",

            });
        }
    }
}