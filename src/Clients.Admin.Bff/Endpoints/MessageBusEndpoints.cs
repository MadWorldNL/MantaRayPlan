using Google.Protobuf.WellKnownTypes;
using MadWorldNL.MantaRayPlan.Api;
using MadWorldNL.MantaRayPlan.MessageBuses;
using Microsoft.AspNetCore.Mvc;

namespace MadWorldNL.MantaRayPlan.Endpoints;

public static class MessageBusEndpoints
{
    public static void AddMessageBusEndpoints(this RouteGroupBuilder endpoints)
    {
        var messageBusEndpoints = endpoints.MapGroup("/MessageBus");
        
        messageBusEndpoints.MapGet("/Status", ([FromServices] MessageBusService.MessageBusServiceClient client) =>
        {
            var status = client.GetStatus(new Empty());
            return new GetStatusResponse(status.Message, status.Counter);
        });
    }
}