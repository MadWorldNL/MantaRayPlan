using MadWorldNL.MantaRayPlan.Events;
using MassTransit;

namespace MadWorldNL.MantaRayPlan.MassTransit;

public class EventPusherConsumer<TMessage> : IConsumer<TMessage> where TMessage : class, IEvent
{
    public Task Consume(ConsumeContext<TMessage> context)
    {
        EventPublisher.OnMessageReceived(context.Message);

        return Task.CompletedTask;
    }
}