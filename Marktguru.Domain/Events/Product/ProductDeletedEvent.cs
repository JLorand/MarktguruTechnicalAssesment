using Marktguru.Domain.Common;

namespace Marktguru.Domain.Events.Product;

public class ProductDeletedEvent : BaseEvent
{
    public Entities.Product Product { get; }

    public ProductDeletedEvent(Entities.Product product)
    {
        Product = product;
    }
}