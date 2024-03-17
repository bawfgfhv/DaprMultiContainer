using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using Dapr;
using BackEnd.IntegrationEvents;
using System.Text.Json;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private const string DAPR_PUBSUB_NAME = "demo.pubsub";
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
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("Ip")]
        public IEnumerable<string> GetIp()
        {
            var ip = NetworkInterface.GetAllNetworkInterfaces().Select(p => p.GetIPProperties())
                .SelectMany(p => p.UnicastAddresses)
                .FirstOrDefault(p =>
                    p.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(p.Address))?.Address
                .ToString();

            return new List<string>() { ip };
        }

        [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToSubmittedIntegrationEvent))]
        public async Task HandleAsync(OrderStatusChangedToSubmittedIntegrationEvent integrationEvent)
        {

            //throw new NotImplementedException("Not OK!");
            var ips = GetIp();
            Console.WriteLine(ips.FirstOrDefault());
            if (integrationEvent.Id != Guid.Empty)
            {
                Console.WriteLine(JsonSerializer.Serialize(integrationEvent));
            }
            else
            {
                _logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", integrationEvent);
            }
        }
    }
}
