using Marktguru.Application.Interfaces;
using Marktguru.Domain.Entities;
using Marktguru.Domain.Events.Product;
using Microsoft.EntityFrameworkCore;

namespace Marktguru.Application.Products.Commands;

public class CreateProductCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<CreateProductCommand, Unit>
{
    public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product()
        {
            Name = request.Name,
            Description = request.Description,
            Availability = request.Availability,
            Price = request.Price,
            DateCreated = DateTime.UtcNow
        };
        
        _ = await dbContext.Products.AddAsync(product, cancellationToken);
        
        product.AddDomainEvent(new ProductCreatedEvent(product));
        
        _ = await dbContext.SaveChangesAsync(cancellationToken);
                
        return await Task.FromResult(Unit.Value);
    }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(v => v.Name).MaximumLength(255).WithMessage("Name must not exceed 255 characters.");
        RuleFor(v => v.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(v => v.Name).MustAsync(async (name, cancellationToken) =>
        {
            return await dbContext.Products.AllAsync(p => p.Name != name, cancellationToken);
        }).WithMessage("The specified name already exists.");
        RuleFor(v => v.Description).MaximumLength(255);
        RuleFor(v => v.Price).GreaterThanOrEqualTo(0);
    }
}

public record CreateProductCommand(
    string Name,
    bool Availability,
    decimal Price,
    string Description) : IRequest<Unit>;