using MadWorldNL.MantaRayPlan.Process;

namespace MadWorldNL.MantaRayPlan.MessageBuses;

public class MessageBusStatusEvent() : IEvent
{
    public int Count { get; set; }
}