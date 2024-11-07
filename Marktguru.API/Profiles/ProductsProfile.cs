using AutoMapper;
using Marktguru.API.Models;
using Marktguru.Application.Products.Queries;

namespace Marktguru.API.Profiles;

public class ProductsProfile : Profile
{
    public ProductsProfile()
    {
        CreateMap<GetProductsQueryResponse, ProductsViewModel>();
        CreateMap<GetProductQueryResponse, ProductViewModel>();
    }
}