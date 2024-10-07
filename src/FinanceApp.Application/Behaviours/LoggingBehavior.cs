using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceApp.Application.Behaviours;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle request={Request} - RequestData={RequestData}",
            typeof(TRequest).Name, request);

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();
        var timeTaken = timer.Elapsed;

        if (timeTaken.TotalSeconds > 3) // Use TotalSeconds for more accuracy
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds.", typeof(TRequest).Name, timeTaken.TotalSeconds);

        logger.LogInformation("[END] Handled {Request}", typeof(TRequest).Name);
        return response;
    }
}