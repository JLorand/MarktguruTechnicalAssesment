using Marktguru.Domain.Events.Product;
using Microsoft.Extensions.Caching.Memory;

namespace Marktguru.Application.Products.EventHandlers;

public class ProductDeletedEventHandler(IMemoryCache memoryCache, ILogger<ProductDeletedEventHandler> logger)
    : INotificationHandler<ProductDeletedEvent>
{
    private readonly ILogger<ProductDeletedEventHandler> _logger = logger;

    public Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
    {
        memoryCache.Remove($"ProductId-{notification.Product.Id}");
        memoryCache.Remove("Products");

        return Task.CompletedTask;
    }
}