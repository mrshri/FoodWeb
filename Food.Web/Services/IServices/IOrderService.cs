using Food.Web.Models;

namespace Food.Web.Services.IServices
{
    public interface IOrderService
    {
        Task<ResponseDto?> CreateOrder(CartDto cartDto);
        Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto);
        Task<ResponseDto?> ValidateStripeSession( int orderHeaderId);
        Task<ResponseDto?>GetAllOrders(string? userId);
        Task<ResponseDto?> GetOrder(int orderId);
        Task<ResponseDto?> UpdateOderStatus(int orderId,string newStatus);

    }
}
