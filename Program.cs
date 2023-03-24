using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MTPerformance;
using MTPerformance.States;

public class Program
{
    public static async Task Main(string[] args) =>
        await CreateHostBuilder(args).Build().RunAsync();

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureLogging((ctx, log) =>
            {
                log.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
            })
            .ConfigureServices((hostBuilderContext, services) =>
            {
                services.AddHostedService<DataGenerationService>();
                services.AddOptions<MassTransitHostOptions>().Configure(conf =>
                {
                    conf.WaitUntilStarted = true;
                });
                services.AddMassTransit(conf =>
                {
                    conf.SetKebabCaseEndpointNameFormatter();
                    
                    var assembly = Assembly.GetExecutingAssembly();
                    
                    conf.AddConsumers(assembly);
                    conf.AddActivities(assembly);
                    conf.AddSagaStateMachine<AnalyticsStateMachine, AnalyticsState>()
                        .InMemoryRepository();

                    conf.UsingRabbitMq((ctx, cfg) =>
                    {
                        cfg.Host("localhost", "/", h =>
                        {
                            h.Username("guest");
                            h.Password("guest");
                        });
                        
                        // always needs to be the last thing called
                        cfg.ConfigureEndpoints(ctx);
                    });
                });
            });
}