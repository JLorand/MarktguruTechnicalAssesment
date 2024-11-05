namespace Marktguru.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest>(ILogger<TRequest> logger) : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger _logger = logger;

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Marktguru Request: {Name} {@Request}", requestName, request);
        
        await Task.CompletedTask;
    }
}