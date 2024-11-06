using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Marktguru.API.Controllers;

[ApiController]
public class ApiControllerBase : ControllerBase
{
    private ILogger? _logger;
    private ISender? _mediator;
    private IMapper? _mapper;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected ILogger Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
        .CreateLogger(HttpContext.Request.RouteValues["controller"]?.ToString()!);

    protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetRequiredService<IMapper>();
}