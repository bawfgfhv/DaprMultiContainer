using FrontEnd.Model;
using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace FrontEnd.IntegrationEvents.Events;

public record UserCheckoutAcceptedIntegrationEvent(
    string UserId,
    string UserEmail,
    string City,
    string Street,
    string State,
    string Country,
    string CardNumber,
    string CardHolderName,
    DateTime CardExpiration,
    string CardSecurityNumber,
    Guid RequestId,
    CustomerBasket Basket)
    : IntegrationEvent;
