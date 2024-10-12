using AutoMapper;
using Food.Services.CouponAPI.Models;
using Food.Services.CouponAPI.Models.DTO;

namespace Food.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDTO, Coupon>();
                config.CreateMap<Coupon, CouponDTO>();                
            });
            return mappingConfig;
        }
    }
}
