using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;

namespace RabbitSender
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Rabbit Sender");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            // otwarcie połączenia
            using (var connection = factory.CreateConnection())
            {
                // utworzenie kanału komunikacji
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "msgKey",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    //console.WriteLine("Wprowadz wiadomość do wysłania: ");
                    for (int i = 0; i < 200; i++)
                    {
                        string msg = "message " + i; //Console.ReadLine();
                        var msgBody = Encoding.UTF8.GetBytes(msg);
                        channel.BasicPublish(exchange: "",
                            routingKey: "msgKey",
                            basicProperties: null,
                            body: msgBody);
                        Console.WriteLine($" [x] wysłano '{msg}'");

                        //Console.WriteLine("ConsumerCount=" + channel.ConsumerCount("msgKey"));


                        Thread.Sleep(2000);
                    }
                }

                Console.WriteLine("...press [Enter] to end");
                Console.ReadLine();
            }
        }
    }
}