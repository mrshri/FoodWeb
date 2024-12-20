﻿using Food.Web.Models;
using Food.Web.Services.IServices;
using static Food.Web.Utilities.StaticDetails;

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
                Url = CouponAPIBase + "/api/coupon/",
            });
        }

        public async Task<ResponseDto> DeleteCouponsAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = CouponAPIBase + "/api/coupon/" + id,
            });
        }
        

        public async Task<ResponseDto> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = CouponAPIBase + "/api/coupon",
            });
        }

        public async Task<ResponseDto> GetCouponByCodeAsync(string CouponCode)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType =ApiType.GET,
                Url = CouponAPIBase + "/api/coupon/GetByCode" + CouponCode,
            }
                );
        }

        public async Task<ResponseDto> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType =ApiType.GET,
                Url =CouponAPIBase + "/api/coupon/" + id,
            });
        }

        public async Task<ResponseDto> UpdateCouponAsync(CouponDTO couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.PUT,
                Data = couponDto,
                Url = CouponAPIBase + "/api/coupon/",
            });
        }
    }
}
