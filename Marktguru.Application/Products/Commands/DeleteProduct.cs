using Marktguru.Application.Common.Exceptions;
using Marktguru.Application.Interfaces;
using Marktguru.Domain.Events.Product;
using Microsoft.EntityFrameworkCore;

namespace Marktguru.Application.Products.Commands;

public class DeleteProductCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteProductCommand, Unit>
{
    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                      ?? throw new NotFoundException(request.Id);
        
        _ = dbContext.Products.Remove(product);
        
        product.AddDomainEvent(new ProductDeletedEvent(product));
        
        _ = dbContext.SaveChangesAsync(cancellationToken);
        
        return await Task.FromResult(Unit.Value);
    }
}

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator(IApplicationDbContext dbContext)
    {
    }
}

public record DeleteProductCommand(Guid Id) : IRequest<Unit>;