using MadWorldNL.MantaRayPlan.Events;
using MadWorldNL.MantaRayPlan.MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace MadWorldNL.MantaRayPlan.Hubs;

public class EventHandlerService
{
    private readonly IHubContext<EventsHub> _context;

    public EventHandlerService(IHubContext<EventsHub> context)
    {
        _context = context;
        
        EventPublisher.OnMessageReceived += SendEventToClient;
    }
    
    private void SendEventToClient(IEvent newEvent)
    {
        _context.Clients.All.SendAsync("NewEvent", newEvent);
    }
}