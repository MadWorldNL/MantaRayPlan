using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MadWorldNL.MantaRayPlan.MessageBuses;
using MantaRayPlan;
using MassTransit;

namespace MadWorldNL.MantaRayPlan.Services;

public class MessageBusServiceProxy(
    IRequestClient<GetMessageBusStatus> getMessageBusStatusClient,
    ILogger<MessageBusServiceProxy> logger)
    : MessageBusService.MessageBusServiceBase
{
    public override async Task<MessageBusStatusReply> GetStatus(Empty request, ServerCallContext context)
    {
        try
        {
            var status =
                await getMessageBusStatusClient.GetResponse<MessageBusStatus>(new GetMessageBusStatus());

            return new MessageBusStatusReply()
            {
                Counter = status.Message.Count,
            };
        }
        catch (RequestFaultException exception) when (exception.Fault?.Exceptions.Any(ex =>
                                                          ex.InnerException?.ExceptionType ==
                                                          "Npgsql.NpgsqlException") ?? false)
        {
            logger.LogError(exception, "Unable to connect with database");

            return new MessageBusStatusReply()
            {
                Message = "Unable to connect with database"
            };
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unknown error");

            return new MessageBusStatusReply()
            {
                Message = "Unknown error"
            };
        }
    }
}