using AutoMapper;
using AutoMapper.QueryableExtensions;
using Marktguru.Application.Interfaces;
using Marktguru.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Marktguru.Application.Products.Queries;

public class GetProductsQueryHandler(IApplicationDbContext dbContext, IMapper mapper, IMemoryCache memoryCache)
    : IRequestHandler<GetProductsQuery, IEnumerable<GetProductsQueryResponse?>>
{
    public async Task<IEnumerable<GetProductsQueryResponse?>> Handle(GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        if (memoryCache.TryGetValue("Products", out IEnumerable<GetProductsQueryResponse>? products))
        {
            return products!;
        }

        products = await dbContext.Products
            .ProjectTo<GetProductsQueryResponse>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        if (products.Any())
        {
            memoryCache.Set("Products", products);
        }
        
        return products;
    }
}

public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    { }
}

public record GetProductsQuery : IRequest<IEnumerable<GetProductsQueryResponse>>;

public record GetProductsQueryResponse(Guid Id, string Name, decimal Price);

public class GetProductsQueryResponseProfile : Profile
{
    public GetProductsQueryResponseProfile()
    {
        CreateMap<Product, GetProductsQueryResponse>();
    }
}