﻿using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Identity.IntegrationEvents;

/// <inheritdoc />
public record OrderStatusChangedToSubmittedIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string BuyerId)
    : IntegrationEvent;
