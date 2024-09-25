using MadWorldNL.MantaRayPlan.Events;
using MadWorldNL.MantaRayPlan.MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace MadWorldNL.MantaRayPlan.Hubs;

public class EventsHub : Hub
{
    public EventsHub()
    {
        EventPublisher.OnMessageReceived += SendEventToClient;
    }
    
    void SendEventToClient(IEvent newEvent)
    {
        
    }

    public new void Dispose()
    {
        EventPublisher.OnMessageReceived -= SendEventToClient;
        
        base.Dispose();
    }
}