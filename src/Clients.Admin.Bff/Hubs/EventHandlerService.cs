using MadWorldNL.MantaRayPlan.Events;
using MadWorldNL.MantaRayPlan.MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace MadWorldNL.MantaRayPlan.Hubs;

public sealed class EventHandlerService : IDisposable
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

    public void Dispose()
    {
        EventPublisher.OnMessageReceived -= SendEventToClient;
        GC.SuppressFinalize(this);
    }
}