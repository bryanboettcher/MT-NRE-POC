using MassTransit;
using MassTransit.Courier.Contracts;

namespace MTPerformance.Contracts;

public class GeocodeSucceeded : RoutingSlipActivityCompleted
{
    public Guid CorrelationId { get; init; }
    
    public Guid TrackingNumber { get; }
    public Guid ExecutionId { get; }
    public DateTime Timestamp { get; }
    public TimeSpan Duration { get; }
    public string? ActivityName { get; }
    public HostInfo? Host { get; }
    public IDictionary<string, object>? Arguments { get; }
    public IDictionary<string, object>? Data { get; }
    public IDictionary<string, object>? Variables { get; }
}