using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Food.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly string _connectionString;

        public MessageBus(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task PublishMessage(object message, string topic_queue_name)
        {
            await using var client = new ServiceBusClient(_connectionString);

            ServiceBusSender sender = client.CreateSender(topic_queue_name);

            var jsonMesaage = JsonConvert.SerializeObject(message);
            ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8
                .GetBytes(jsonMesaage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(finalMessage);
            await client.DisposeAsync();
        }
    }
}
