using MassTransit;

namespace MTPerformance.States;

public class AnalyticsState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public int Version { get; set; }

    public string? CurrentState { get; set; }
    public int ReadyEventStatus { get; set; }   // need a better name here


    public int Zipcode { get; set; }

    public float? Latitude { get; set; }
    public float? Longitude { get; set; }
}