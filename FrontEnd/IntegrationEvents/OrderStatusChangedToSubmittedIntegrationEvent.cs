using Microsoft.eShopOnDapr.BuildingBlocks.EventBus.Events;

namespace FrontEnd.IntegrationEvents;

public record OrderStatusChangedToSubmittedIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string BuyerId)
    : IntegrationEvent;
