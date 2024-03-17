using FrontEnd.IntegrationEvents.Events;
using FrontEnd.Model;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;

namespace FrontEnd.IntegrationEvents.EventHandling;

public class OrderStatusChangedToSubmittedIntegrationEventHandler
    : IIntegrationEventHandler<OrderStatusChangedToSubmittedIntegrationEvent>
{
    private readonly IBasketRepository _repository;

    public OrderStatusChangedToSubmittedIntegrationEventHandler(
        IBasketRepository repository)
    {
        _repository = repository;
    }

    public Task Handle(OrderStatusChangedToSubmittedIntegrationEvent @event)
    {
        Console.WriteLine($@"I'm a {nameof(OrderStatusChangedToSubmittedIntegrationEventHandler)}!");
        return _repository.DeleteBasketAsync(@event.BuyerId);
    }
}



