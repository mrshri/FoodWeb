using Food.Web.Models;
using Food.Web.Models;
using Food.Web.Services.IServices;
using Food.Web.Utilities;
using static Food.Web.Utilities.StaticDetails;
using System;

namespace Food.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
            public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto> CreateCouponAsync(CouponDTO couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = couponDto,
                Url = StaticDetails.CouponAPIBase + "/api/coupon/",
            });
        }

        public async Task<ResponseDto> DeleteCouponsAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.CouponAPIBase + "/api/coupon/" + id,
            });
        }
        

        public async Task<ResponseDto> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.CouponAPIBase + "/api/coupon",
            });
        }

        public async Task<ResponseDto> GetCouponByCodeAsync(string CouponCode)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.CouponAPIBase + "/api/coupon/GetByCode" + CouponCode,
            }
                );
        }

        public async Task<ResponseDto> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.CouponAPIBase + "/api/coupon/" + id,
            });
        }

        public async Task<ResponseDto> UpdateCouponAsync(CouponDTO couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = couponDto,
                Url = StaticDetails.CouponAPIBase + "/api/coupon/",
            });
        }
    }
}
