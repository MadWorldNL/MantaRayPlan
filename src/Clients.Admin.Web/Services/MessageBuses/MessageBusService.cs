using MadWorldNL.MantaRayPlan.MessageBuses;

namespace MadWorldNL.MantaRayPlan.Web.Services.MessageBuses;

public class MessageBusService : IMessageBusService
{
    public GetStatusResponse GetStatus()
    {
        return new GetStatusResponse("", 10);
    }

    public PostStatusResponse PostNewStatus()
    {
        return new PostStatusResponse("");
    }
}