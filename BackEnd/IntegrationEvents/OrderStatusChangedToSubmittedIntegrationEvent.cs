﻿using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace BackEnd.IntegrationEvents;

public record OrderStatusChangedToSubmittedIntegrationEvent(
    Guid OrderId,
    string OrderStatus,
    string BuyerId)
    : IntegrationEvent;
