using System;

namespace RabbitMQAuthenticationExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var eventProducer = new EventProducer();

            try
            {
                eventProducer.PublishMessage("Hello RabbitMQ");
                Console.WriteLine("Message published successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error publishing message: " + ex.Message);
            }

            Console.ReadLine();
        }
    }
}