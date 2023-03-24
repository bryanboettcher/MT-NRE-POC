using MassTransit;
using MassTransit.Courier.Contracts;
using MTPerformance.Contracts;

namespace MTPerformance.Machinery
{
    public class BeginGeocodingConsumer : IConsumer<BeginGeocoding>
    {
        public async Task Consume(ConsumeContext<BeginGeocoding> context)
        {
            var router = new RoutingSlipBuilder(NewId.NextGuid());

            router.SetVariables(new
            {
                context.Message.Zipcode
            });

            await HandleGeocoding(context, router);

            var slip = router.Build();
            await context.Execute(slip);
        }
        
        private static async Task HandleGeocoding(ConsumeContext<BeginGeocoding> context, RoutingSlipBuilder router)
        {
            const string activityName = "HandleGeocoding";

            router.AddActivity(activityName, new Uri("exchange:handle-geocoding_execute"));
            await router.AddSubscription(
                context.SourceAddress,
                RoutingSlipEvents.ActivityFaulted,
                RoutingSlipEventContents.None,
                activityName,
                x => x.Send<GeocodeFailure>(new { context.Message.CorrelationId })
            );
            await router.AddSubscription(
                context.SourceAddress,
                RoutingSlipEvents.ActivityCompleted,
                RoutingSlipEventContents.All,
                activityName,
                x => x.Send<GeocodeSucceeded>(new { context.Message.CorrelationId })
            );
        }
    }
}
