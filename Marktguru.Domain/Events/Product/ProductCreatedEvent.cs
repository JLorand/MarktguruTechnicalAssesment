using Marktguru.Domain.Common;

namespace Marktguru.Domain.Events.Product;

public class ProductCreatedEvent : BaseEvent
{
    public Entities.Product Product { get; }

    public ProductCreatedEvent(Entities.Product product)
    {
        Product = product;
    }
}