using Food.Web.Models;

namespace Food.Web.Services.IServices
{
    public interface IShoppingCartService
    {
        Task<ResponseDto> GetCartByUserIDAsync(string userId);
        Task<ResponseDto> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto> RemoveFromCartAsync(int CartDetailsId);
        Task<ResponseDto> ApplyCouponAsync(CartDto cartDto);
    }
}
