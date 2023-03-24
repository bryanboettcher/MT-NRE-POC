using MassTransit;
using MTPerformance.Contracts;

namespace MTPerformance.States
{
    public class AnalyticsStateMachine : MassTransitStateMachine<AnalyticsState>
    {
        public AnalyticsStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Initially(
                When(ProcessingStarted).Then(context =>
                {
                    context.Saga.CorrelationId = context.Message.CorrelationId;
                    context.Saga.Zipcode = context.Message.Zipcode;
                })
                .PublishAsync(context => context.Init<BeginGeocoding>(context.Message))
                .TransitionTo(Created)
            );

            During(Created,
                When(GeocodingCompleted).Then(context =>
                {
                    context.Saga.Latitude = context.GetVariable<float>("Latitude");
                    context.Saga.Longitude = context.GetVariable<float>("Longitude");
                }).TransitionTo(Completed),
                When(GeocodingFailed).TransitionTo(Invalid)
            );

            During(Invalid, Ignore(GeocodingCompleted));
        }
        
        // ReSharper disable UnassignedGetOnlyAutoProperty
        // ReSharper disable MemberCanBePrivate.Global

        public State Created { get; }
        public State Invalid { get; }
        public State Completed { get; }

        public Event<Initialize> ProcessingStarted { get; }
        
        public Event<GeocodeFailure> GeocodingFailed { get; }
        public Event<GeocodeSucceeded> GeocodingCompleted { get; }
    }
}
