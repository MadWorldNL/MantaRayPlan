using Microsoft.AspNetCore.SignalR;
using SignalRSwaggerGen.Attributes;

namespace MadWorldNL.MantaRayPlan.Hubs;

[SignalRHub]
// The 'EventHandlerService' needs to be injected by DI but does not need to be used directly.
#pragma warning disable CS9113 // Parameter is unread.
public class EventsHub(EventHandlerService _) : Hub
#pragma warning restore CS9113 // Parameter is unread.
{
}