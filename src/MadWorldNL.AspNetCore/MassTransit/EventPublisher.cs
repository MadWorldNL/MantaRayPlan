using MadWorldNL.MantaRayPlan.Events;

namespace MadWorldNL.MantaRayPlan.MassTransit;

public static class EventPublisher
{
    public static Action<IEvent> OnMessageReceived { get; set; } = _ => { };
}