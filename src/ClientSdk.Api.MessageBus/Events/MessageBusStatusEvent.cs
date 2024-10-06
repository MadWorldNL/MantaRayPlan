namespace MadWorldNL.MantaRayPlan.Events;

public class MessageBusStatusEvent() : IEvent
{
    public int Count { get; set; }
}