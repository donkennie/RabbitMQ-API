using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQAuthenticationExample
{
    public class EventProducer
    {
        private readonly string _hostname = "localhost";
        private readonly int _port = 5672;
        private readonly string _username = "guest";
        private readonly string _password = "guest";
        private readonly string _exchangeName = "my_exchange";
        private readonly string _routingKey = "my_routing_key";

        public async Task PublishMessage(string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                Port = _port,
                UserName = _username,
                Password = _password
            };

            using (var connection = await factory.CreateConnectionAsync())
            using (var channel = await connection.CreateChannelAsync())
            {
                var messageBody = Encoding.UTF8.GetBytes(message);

                await channel.BasicPublishAsync(
                    exchange: _exchangeName,
                    routingKey: _routingKey,
                    body: messageBody
                );

                Console.WriteLine("Event published:{0}", message);
            }
        }
    }
}