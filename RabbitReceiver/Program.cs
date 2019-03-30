using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitReceiver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Rabbit Receiver");

            //Console.WriteLine("...press [Enter] to read");
            //Console.ReadLine();

            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
            // otwarcie połączenia
            IConnection connection = factory.CreateConnection();

            // utworzenie kanału komunikacji
            IModel channel = connection.CreateModel();

            channel.QueueDeclare(queue: "msgKey",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            EventingBasicConsumer consumer = CreateConsumer(channel);

           // Thread.Sleep(10000);

            //Console.WriteLine("...press [Enter] to end");
            //Console.ReadLine();
        }

        public static EventingBasicConsumer CreateConsumer(IModel channel)
        {
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

            consumer.Shutdown += (d, e) =>
            {
                Console.WriteLine($"ShutdownReason '{consumer.ShutdownReason}'");
                //consumer = CreateConsumer(channel);
            };
            Thread.Sleep(1000);

            // for (int i = 0; i < 10; i++)
            {
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] otrzymano '{message}'");
                    // Console.WriteLine("ConsumerCount=" + channel.MessageCount("msgKey"));
                   // Thread.Sleep(100);
                };

                channel.BasicConsume(queue: "msgKey",
                    autoAck: true,
                    consumer: consumer);

                return (consumer);
            }
        }
    }
}