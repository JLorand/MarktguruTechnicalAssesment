using Marktguru.Domain.Events.Product;
using Microsoft.Extensions.Caching.Memory;

namespace Marktguru.Application.Products.EventHandlers;

public class ProductUpdatedEventHandler(IMemoryCache memoryCache, ILogger<ProductUpdatedEventHandler> logger)
    : INotificationHandler<ProductUpdatedEvent>
{
    private readonly ILogger<ProductUpdatedEventHandler> _logger = logger;

    public Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        memoryCache.Remove($"ProductId-{notification.Product.Id}");
        memoryCache.Remove("Products");

        return Task.CompletedTask;
    }
}