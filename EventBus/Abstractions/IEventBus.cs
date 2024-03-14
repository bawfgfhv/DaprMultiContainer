using Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Events;

namespace Microsoft.eShopOnContainers.BuildingBlocks.EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent integrationEvent);
}
