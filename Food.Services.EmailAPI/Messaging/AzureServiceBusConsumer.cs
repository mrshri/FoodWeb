using Azure.Messaging.ServiceBus;
using Food.Services.EmailAPI.Message;
using Food.Services.EmailAPI.Models.Dto;
using Food.Services.EmailAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Food.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailshoppingcartQueue;
        private readonly string RegisterUserQueue;
        private readonly string orderCreated_Topic;
        private readonly string orderCreated_Email_Subscription;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        private ServiceBusProcessor _emailShoppingCartProcessor;
        private ServiceBusProcessor _RegisterUserProcessor;
        private ServiceBusProcessor _emailOrderPlaceProcessor;


        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            emailshoppingcartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            RegisterUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue");
            orderCreated_Topic = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreatedTopic");
            orderCreated_Email_Subscription = _configuration.GetValue<string>("TopicAndQueueNames:OrderCreated_Email_Subscription");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailShoppingCartProcessor = client.CreateProcessor(emailshoppingcartQueue);
            _RegisterUserProcessor = client.CreateProcessor(RegisterUserQueue);
            _emailOrderPlaceProcessor = client.CreateProcessor(orderCreated_Topic, orderCreated_Email_Subscription);


        }

        public async Task Start()
        {
          _emailShoppingCartProcessor.ProcessMessageAsync += OnEmailShoppingCartRequeuetReceived;
            _emailShoppingCartProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailShoppingCartProcessor.StartProcessingAsync();

            _RegisterUserProcessor.ProcessMessageAsync += OnUserRegisterRequeuetReceived;
            _RegisterUserProcessor.ProcessErrorAsync += ErrorHandler;
            await _RegisterUserProcessor.StartProcessingAsync();

            _emailOrderPlaceProcessor.ProcessMessageAsync += OnOrderPlacedRequeuetReceived;
            _emailOrderPlaceProcessor.ProcessErrorAsync += ErrorHandler;
            await _emailOrderPlaceProcessor.StartProcessingAsync();
        }

      

        public async Task Stop()
        {
            await _emailShoppingCartProcessor.StopProcessingAsync();
            await _emailShoppingCartProcessor.DisposeAsync();

            await _RegisterUserProcessor.StopProcessingAsync();
            await _RegisterUserProcessor.DisposeAsync();

            await _emailOrderPlaceProcessor.StopProcessingAsync();
            await _emailOrderPlaceProcessor.DisposeAsync();
        }

        private  Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnEmailShoppingCartRequeuetReceived(ProcessMessageEventArgs args)
        {
            //here we will receive and process messages
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                //todo try to log email
                await _emailService.EmailShoppingCartAndLog(objMessage);
                await args.CompleteMessageAsync(args.Message);

            }
            catch (Exception ex)
            {

                throw;
            }
        }


        private async Task OnUserRegisterRequeuetReceived(ProcessMessageEventArgs args)
        {
            //here we will receive and process messages
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            string email = JsonConvert.DeserializeObject<string>(body);
            try
            {
                //todo try to log email
                await _emailService.RegisterUserAndLog(email);
                await args.CompleteMessageAsync(args.Message);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task OnOrderPlacedRequeuetReceived(ProcessMessageEventArgs args)
        {
            //here we will receive and process messages
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            RewardMessage objMessage = JsonConvert.DeserializeObject<RewardMessage>(body);
            try
            {
                //todo try to log email
                await _emailService.LogOrderPlaced(objMessage);
                await args.CompleteMessageAsync(args.Message);

            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }
}
