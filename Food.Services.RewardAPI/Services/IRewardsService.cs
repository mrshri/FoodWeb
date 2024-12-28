using Food.Services.RewardAPI.Message;
namespace Food.Services.RewardAPI.Services
{
    public interface IRewardsService
    {
        Task UpdateRewards(RewardMessage rewardMessage);
        
    }
}
