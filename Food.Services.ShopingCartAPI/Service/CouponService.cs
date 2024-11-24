using Food.Services.ShoppingCartAPI.Models.DTO;
using Food.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Food.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private IHttpClientFactory _httpClientFactory;
        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<CouponDTO> GetCoupon(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var apiResponce = await client.GetAsync($"api/coupon/GetByCode/{couponCode}");
            var apiContent = await apiResponce.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (response != null &&  response.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(response.Result));
            }

            return new CouponDTO();

        }
    }
}
