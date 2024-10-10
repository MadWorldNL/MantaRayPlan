using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MadWorldNL.MantaRayPlan.Api;
using MadWorldNL.MantaRayPlan.MessageBuses;
using MassTransit;

namespace MadWorldNL.MantaRayPlan.Services;

public class MessageBusServiceProxy(
    IRequestClient<MessageBusStatusQuery> getMessageBusStatusClient,
    ISendEndpointProvider sendEndpointProvider,
    ILogger<MessageBusServiceProxy> logger)
    : MessageBusService.MessageBusServiceBase
{
    public override async Task<GetMessageBusStatusReply> GetStatus(Empty request, ServerCallContext context)
    {
        try
        {
            var status =
                await getMessageBusStatusClient.GetResponse<MessageBusStatus>(new MessageBusStatusQuery(), context.CancellationToken);

            return new GetMessageBusStatusReply()
            {
                Counter = status.Message.Count,
            };
        }
        catch (RequestFaultException exception) when (Array.Exists(exception.Fault?.Exceptions ?? [], ex =>
                                                          ex.InnerException?.ExceptionType ==
                                                          "Npgsql.NpgsqlException"))
        {
            logger.LogError(exception, "Unable to connect with database");

            return new GetMessageBusStatusReply()
            {
                Message = "Unable to connect with database"
            };
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unknown error");

            return new GetMessageBusStatusReply()
            {
                Message = "Unknown error"
            };
        }
    }

    public override async Task<PostMessageBusStatusReply> PostStatus(Empty request, ServerCallContext context)
    {
        await sendEndpointProvider.Send(new MessageBusStatusCommand("Test"), context.CancellationToken);
        
        return new PostMessageBusStatusReply();
    }
}