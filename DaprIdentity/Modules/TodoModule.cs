using Carter;
using Dapr.Client;
using DaprIdentity.IntegrationEvents;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace DaprIdentity.Modules
{
    public class TodoModule : ICarterModule
    {
        string DAPR_PUBSUB_NAME = "demo.pubsub";
        string StateStoreName = "demo.statestore";

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/todo/{id}", async (int id, DaprClient daprClient) =>
                {
                    var state = await daprClient.GetStateEntryAsync<TodoItem>(StateStoreName, $"{id}");
                    return $"Todo item id is {id},Name is {state?.Value?.Name}.";
                })
                .WithTags("TodoGroup");


            app.MapPost("/todo/create", async (HttpContext context , [FromBody] TodoItem item, IEventBus eventBus, DaprClient daprClient) =>
            {
               var user = context.User;

               var x2 = context.GetTokenAsync(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme, Destinations.AccessToken);

                await eventBus.PublishAsync(
                    new OrderStatusChangedToSubmittedIntegrationEvent(Guid.NewGuid(), "付款了", "bawfgfhv@qq.com"));

                var state = await daprClient.GetStateEntryAsync<TodoItem>(StateStoreName, $"{item.Id}");
                state.Value = item;
                await state.SaveAsync();

                return Results.Ok(item);
            }).WithTags("TodoGroup");

            app.MapPost("/subscribe", () => { Console.WriteLine("我要咯咯咯，叽叽叽，啦啦啦！"); }).WithTopic(DAPR_PUBSUB_NAME,
                nameof(OrderStatusChangedToSubmittedIntegrationEvent));
        }
    }

    public class TodoItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}