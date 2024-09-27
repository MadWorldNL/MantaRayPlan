using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace MadWorldNL.MantaRayPlan.Hubs;

[SignalRHub]
public class EventsHub(EventHandlerService _) : Hub
{
}