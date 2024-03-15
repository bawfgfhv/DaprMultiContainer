using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace DaprSubDemo.IntegrationEvents;

public record OrderStatusChangedToSubmittedIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string BuyerId)
    : IntegrationEvent;
