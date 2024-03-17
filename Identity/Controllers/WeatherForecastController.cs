using System.Text.Json;
using Dapr;
using Identity.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    /// <inheritdoc />
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private const string DAPR_PUBSUB_NAME = "demo.pubsub";

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        /// <inheritdoc />
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "GetWeatherForecast")]
        [Topic(DAPR_PUBSUB_NAME, nameof(OrderStatusChangedToSubmittedIntegrationEvent))]
        public async Task Get(OrderStatusChangedToSubmittedIntegrationEvent integrationEvent)
        {
            Console.WriteLine(JsonSerializer.Serialize(integrationEvent));
        }
    }
}
