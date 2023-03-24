using MassTransit;

namespace MTPerformance.Machinery;

public class HandleGeocodingActivity : IExecuteActivity<HandleGeocodingActivityArguments>
{
    public async Task<ExecutionResult> Execute(ExecuteContext<HandleGeocodingActivityArguments> context)
    {
        await Task.Delay(2);
        return context.CompletedWithVariables(new
        {
            Latitude = (Random.Shared.NextSingle() * 200) - 100,
            Longitude = (Random.Shared.NextSingle() * 200) - 100,
        });
    }
}