using MassTransit;

namespace MTPerformance.Contracts
{
    [ExcludeFromTopology]
    public class CommonDataset
    {
        public Guid CorrelationId { get; init; }
    };
}
