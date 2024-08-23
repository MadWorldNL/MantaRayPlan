using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MadWorldNL.MantaRayPlan.MessageBuses;
using MantaRayPlan;

namespace MadWorldNL.MantaRayPlan.Services;

public class MessageBusServiceProxy(IMessageBusRepository messageBusRepository, ILogger<MessageBusServiceProxy> logger)
        : MessageBusService.MessageBusServiceBase
{
    public override async Task<MessageBusStatusReply> GetStatus(Empty request, ServerCallContext context)
    {
        try
        {
            var status = await messageBusRepository.FindStatusAsync() ?? new MessageBusStatus();
            
            return new MessageBusStatusReply()
            {
                Counter = status.Count
            };
        }
        catch (Exception ex)
        {
            const string message = "Database error";
            
            logger.LogError(ex, message);
            
            return new MessageBusStatusReply()
            {
                Message = message
            };
        }
    }
}