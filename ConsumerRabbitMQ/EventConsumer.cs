using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;

public class EventConsumer
{
    private readonly string rabbitMqConnectionString;

    public EventConsumer(string rabbitMqConnectionString)
    {
        this.rabbitMqConnectionString = rabbitMqConnectionString;
    }

    public async Task ConsumeEvents(string exchangeName, string queueName, string routingKey)
    {
        var factory = new ConnectionFactory
        {
            Uri = new Uri(rabbitMqConnectionString)
        };

        using (var connection = await factory.CreateConnectionAsync())
        using (var channel = await connection.CreateChannelAsync())
        {
            await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic);
            await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false);
            await channel.QueueBindAsync(queueName, exchangeName, routingKey);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Event received: {message}");

                await Task.CompletedTask;
            };

            await channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: true,
                consumer: consumer
            );

            Console.WriteLine("Waiting for events...");
            await Task.Delay(-1); // keeps app alive
        }
    }
}