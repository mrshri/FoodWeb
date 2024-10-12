using Food.Web.Models;

namespace Food.Web.Services.IServices
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponByCodeAsync(String CouponCode);
        Task<ResponseDto?> GetAllCouponsAsync();
        Task<ResponseDto?> GetCouponByIdAsync(int id);
        Task<ResponseDto?> CreateCouponAsync(CouponDTO couponDto);
        Task<ResponseDto?> UpdateCouponAsync(CouponDTO couponDto);
        Task<ResponseDto?> DeleteCouponsAsync(int id);

    }
}
