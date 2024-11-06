using Marktguru.Domain.Common;

namespace Marktguru.Domain.Events.Product;

public class ProductUpdatedEvent : BaseEvent
{
    public Entities.Product Product { get; }

    public ProductUpdatedEvent(Entities.Product product)
    {
        Product = product;
    }
}