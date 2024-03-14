using Dapr.Client;
using FrontEnd.IntegrationEvents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using Microsoft.eShopOnDapr.BuildingBlocks.EventBus.Abstractions;

namespace FrontEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly DaprClient _daprClient;
        private readonly IEventBus _eventBus;

        public WeatherForecastController(DaprClient daprClient, IEventBus eventBus)
        {
            _daprClient = daprClient;
            _eventBus = eventBus;
        }

        [HttpGet("FrontEnd")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            // 注意 BackEnd 后期要用
            return await _daprClient.InvokeMethodAsync<IEnumerable<WeatherForecast>>(HttpMethod.Get, "BackEnd",
                "WeatherForecast");

        }

        [HttpGet("getIp")]
        public async Task<IEnumerable<string>> getIp()
        {
            var app = await _daprClient.InvokeMethodAsync<IEnumerable<string>>(HttpMethod.Get, "BackEnd",
                "/WeatherForecast/Ip");
            return app;
        }

        [HttpGet("getOtherApp")]
        public async Task<IActionResult> GetOtherApp()
        {
            await _eventBus
                .PublishAsync(new OrderStatusChangedToSubmittedIntegrationEvent(Guid.NewGuid(), "Begin", "liu"))
                .ConfigureAwait(false);

            return Ok("OK");
        }
    }
}