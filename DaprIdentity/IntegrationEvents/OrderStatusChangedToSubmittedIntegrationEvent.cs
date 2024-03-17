using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace DaprIdentity.IntegrationEvents;

/// <inheritdoc />
public record OrderStatusChangedToSubmittedIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string BuyerId)
    : IntegrationEvent;
