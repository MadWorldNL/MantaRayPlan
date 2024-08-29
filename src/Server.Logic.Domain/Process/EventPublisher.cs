namespace MadWorldNL.MantaRayPlan.Process;

public static class EventPublisher
{
    public static Action<IEvent> OnMessageReceived { get; set; } = _ => { };
}