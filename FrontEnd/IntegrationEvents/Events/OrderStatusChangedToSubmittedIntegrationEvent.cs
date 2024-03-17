using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace FrontEnd.IntegrationEvents.Events;

public record OrderStatusChangedToSubmittedIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string BuyerId)
    : IntegrationEvent;
