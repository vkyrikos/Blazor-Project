using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlazorApp.Service;

public sealed class Service(ILogger<Service> logger) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"{nameof(Service)} started");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"{nameof(Service)} stopped");

        return Task.CompletedTask;
    }
}
