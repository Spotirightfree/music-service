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
        [Route("SendMessageDeleteSong")]
        public void sendMessage()
        {
            //nummer wordt verwijderd -> verwijder uit alle playlists
            var factory = new ConnectionFactory { HostName = "192.168.240.6" };
            factory.UserName = "main-service";
            factory.Password = "main-service";
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            const string message = "Delete Song X";

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "main-exchange",
                                 routingKey: "deleteSong",
                                 body: body);
        }

        [HttpGet]
        [Route("Hello_World")]
        public string HelloWorld()
        {
            return "Hello World from Music-Service";
        }

        [HttpGet]
        [Route("Crach_Test")]
        public string CrashTest()
        {
            throw new Exception("MEGA CRASH~!");
        }
    }
}