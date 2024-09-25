using System.Text.Json;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MadWorldNL.MantaRayPlan.Api;
using MadWorldNL.MantaRayPlan.Events;
using MadWorldNL.MantaRayPlan.MassTransit;

namespace MadWorldNL.MantaRayPlan.Services;

public class EventServiceProxy : EventService.EventServiceBase
{
    public override Task Subscribe(Empty request, IServerStreamWriter<newEvent> responseStream, ServerCallContext context)
    {
        EventPublisher.OnMessageReceived += SendEventToClient;
        
        while (!context.CancellationToken.IsCancellationRequested)
        {
            Thread.Sleep(1000);
        }
        
        EventPublisher.OnMessageReceived -= SendEventToClient;
        return Task.CompletedTask;
        
        void SendEventToClient(IEvent newEvent)
        {
            var newEventResponse = new newEvent()
            {
                Type = newEvent.GetType().Name,
                Json = JsonSerializer.Serialize(newEvent, newEvent.GetType())
            };
            
            responseStream.WriteAsync(newEventResponse);
        }
    }
}