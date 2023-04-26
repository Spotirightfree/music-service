using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace music_service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        [Route("SendMessagePlaylist")]
        public void sendMessage()
        {
            var factory = new ConnectionFactory { HostName = "192.168.240.6" };
            factory.UserName = "playlist-service";
            factory.Password = "playlist-service";
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "playlist-service-queue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            const string message = "Hello World!";

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "playlist-service-queue",
                                 basicProperties: null,
                                 body: body);
        }

        [HttpPost]
        [Route("SendMessageLogin")]
        public void sendMessageLogin()
        {
            var factory = new ConnectionFactory { HostName = "192.168.240.6" };
            factory.UserName = "playlist-service";
            factory.Password = "playlist-service";
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(queue: "login-service-queue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            const string message = "Hello World!";

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: "login-service-queue",
                                 basicProperties: null,
                                 body: body);
        }

        [HttpGet]
        [Route("Hello_World")]
        public string HelloWorld()
        {
            return "Hello World from Music-Service";
        }
    }
}