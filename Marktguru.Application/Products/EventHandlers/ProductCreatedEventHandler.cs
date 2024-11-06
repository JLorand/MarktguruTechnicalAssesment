using Marktguru.Domain.Events.Product;
using Microsoft.Extensions.Caching.Memory;

namespace Marktguru.Application.Products.EventHandlers;

public class ProductCreatedEventHandler(IMemoryCache memoryCache, ILogger<ProductCreatedEventHandler> logger)
    : INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger = logger;

    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        memoryCache.Remove($"ProductId-{notification.Product.Id}");
        memoryCache.Remove("Products");

        return Task.CompletedTask;
    }
}