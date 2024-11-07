using AutoMapper;
using AutoMapper.QueryableExtensions;
using Marktguru.Application.Interfaces;
using Marktguru.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Marktguru.Application.Products.Queries;

public class GetProductQueryHandler(IApplicationDbContext dbContext, IMapper mapper, IMemoryCache memoryCache)
    : IRequestHandler<GetProductQuery, GetProductQueryResponse?>
{
    public async Task<GetProductQueryResponse?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        return await memoryCache.GetOrCreateAsync($"ProductId-{request.Id}", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            return await dbContext.Products
                .Where(p => p.Id == request.Id)
                .ProjectTo<GetProductQueryResponse>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        });
    }
}

public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
{
    public GetProductQueryValidator()
    {
        RuleFor(v => v.Id).NotEmpty();
    }
}

public record GetProductQuery(Guid Id) : IRequest<GetProductQueryResponse>;

public record GetProductQueryResponse(
    Guid Id,
    string Name,
    bool Availability,
    decimal Price,
    string Description,
    DateTime DateCreated);

public class GetProductQueryResponseProfile : Profile
{
    public GetProductQueryResponseProfile()
    {
        CreateMap<Product, GetProductQueryResponse>();
    }
}