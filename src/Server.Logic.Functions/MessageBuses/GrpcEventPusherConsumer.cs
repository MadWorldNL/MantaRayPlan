using MadWorldNL.MantaRayPlan.Process;
using MassTransit;

namespace MadWorldNL.MantaRayPlan.MessageBuses;

public class GrpcEventPusherConsumer<TMessage> : IConsumer<TMessage> where TMessage : class, IEvent
{
    public Task Consume(ConsumeContext<TMessage> context)
    {
        EventPublisher.OnMessageReceived(context.Message);

        return Task.CompletedTask;
    }
}