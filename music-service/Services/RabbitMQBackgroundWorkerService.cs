using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Channels;

namespace music_service.Services
{
    public class RabbitMQBackgroundWorkerService : BackgroundService
    {
        readonly ILogger<RabbitMQBackgroundWorkerService> _logger;

        public RabbitMQBackgroundWorkerService(ILogger<RabbitMQBackgroundWorkerService> logger)
        {
            this._logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() =>
            {
                var factory = new ConnectionFactory { HostName = "192.168.240.6" };
                factory.UserName = "playlist-service";
                factory.Password = "playlist-service";
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: "music-service-queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                Console.WriteLine(" [*] Waiting for messages.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine($" [x] Received {message}");
                };
                channel.BasicConsume(queue: "music-service-queue",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            });
            
        }



    }
}
