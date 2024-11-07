using Marktguru.Application.Common.Exceptions;
using Marktguru.Application.Interfaces;
using Marktguru.Domain.Entities;
using Marktguru.Domain.Events.Product;
using Microsoft.EntityFrameworkCore;

namespace Marktguru.Application.Products.Commands;

public class UpdateProductCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<UpdateProductCommand, Unit>
{
    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
                      ?? throw new NotFoundException(request.Id);

        product.Name = request.Name;
        product.Availability = request.Availability;
        product.Price = request.Price;
        product.Description = request.Description;

        _ = dbContext.Products.Update(product);

        product.AddDomainEvent(new ProductUpdatedEvent(product));

        try
        {
            _ = await dbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException e)
        {
            await HandleConcurrencyException(e, cancellationToken);
        }

        return await Task.FromResult(Unit.Value);
    }

    private static async Task HandleConcurrencyException(DbUpdateConcurrencyException e,
        CancellationToken cancellationToken)
    {
        foreach (var entry in e.Entries)
        {
            if (entry.Entity is not Product) continue;
            var proposedValues = entry.CurrentValues;
            var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);

            foreach (var property in proposedValues.Properties)
            {
                var proposedValue = proposedValues[property];
                var databaseValue = databaseValues?[property];

                // Decide which value should be written to database
                proposedValues[property] = proposedValue;
            }

            // Refresh original values to bypass next concurrency check
            if (databaseValues is not null)
            {
                entry.OriginalValues.SetValues(databaseValues);
            }
        }
    }
}

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator(IApplicationDbContext dbContext)
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

public record UpdateProductCommand(
    Guid Id,
    string Name,
    bool Availability,
    decimal Price,
    string Description) : IRequest<Unit>;