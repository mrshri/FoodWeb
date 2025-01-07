using Food.Services.EmailAPI.Message;
using Food.Services.EmailAPI.Models.Dto;

namespace Food.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailShoppingCartAndLog(CartDto cartDto);
        Task RegisterUserAndLog(string email);
        Task LogOrderPlaced(RewardMessage rewardsDto);
    }
}
