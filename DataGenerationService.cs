using MassTransit;
using Microsoft.Extensions.Hosting;
using MTPerformance.Contracts;

namespace MTPerformance
{
    public class DataGenerationService : BackgroundService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        
        public DataGenerationService(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _publishEndpoint.Publish(new Initialize { CorrelationId = NewId.NextGuid(), Zipcode = 90210 }, stoppingToken);

            Console.WriteLine("Publish completed");
            await Task.Delay(-1, stoppingToken);
        }
    }
}