using Azure.Messaging.ServiceBus;
using Food.Services.RewardAPI.Message;
using Food.Services.RewardAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Food.Services.RewardAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string ordercreatedTopic;
        private readonly string orderCreatedRewardsSubscription;
        private readonly IConfiguration _configuration;
        private readonly RewardsService _rewardsService;

        private ServiceBusProcessor _rewardProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration, RewardsService rewardsService)
        {
            _configuration = configuration;
            _rewardsService = rewardsService;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            ordercreatedTopic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            orderCreatedRewardsSubscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Rewards_Subscription");


            var client = new ServiceBusClient(serviceBusConnectionString);
            _rewardProcessor = client.CreateProcessor(ordercreatedTopic,orderCreatedRewardsSubscription);

        }

        public async Task Start()
        {
            _rewardProcessor.ProcessMessageAsync += OnNewRewardsRequestReceived;
            _rewardProcessor.ProcessErrorAsync += ErrorHandler;
            await _rewardProcessor.StartProcessingAsync();


        }


        public async Task Stop()
        {
            await _rewardProcessor.StopProcessingAsync();
            await _rewardProcessor.DisposeAsync();

       
        }

        private  Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnNewRewardsRequestReceived(ProcessMessageEventArgs args)
        {
            //here we will receive and process messages
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            RewardMessage objMessage = JsonConvert.DeserializeObject<RewardMessage>(body);
            try
            {
                //todo try to log email
                await _rewardsService.UpdateRewards(objMessage);
                await args.CompleteMessageAsync(args.Message);

            }
            catch (Exception ex)
            {

                throw;
            }
        }    


    }
}
