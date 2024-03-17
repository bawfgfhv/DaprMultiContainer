using Dapr.Client;
using FrontEnd.IntegrationEvents;
using FrontEnd.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using System.Net;
using FrontEnd.Services;

namespace FrontEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly DaprClient _daprClient;
        private readonly IEventBus _eventBus;
        private readonly IBasketRepository _basketRepository;
        private readonly IIdentityService _identityService;

        /// <inheritdoc />
        public WeatherForecastController(DaprClient daprClient, IEventBus eventBus, IBasketRepository basketRepository, IIdentityService identityService)
        {
            _daprClient = daprClient;
            _eventBus = eventBus;
            _basketRepository = basketRepository;
            _identityService = identityService;
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

        [HttpPost("UpdateBasket")]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] CustomerBasket value)
        {
            var userId = _identityService.GetUserIdentity();

            value.BuyerId = string.IsNullOrWhiteSpace(userId) ? Guid.NewGuid().ToString("N") : userId;

            return Ok(await _basketRepository.UpdateBasketAsync(value));
        }
    }
}