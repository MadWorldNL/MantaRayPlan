using MadWorldNL.MantaRayPlan.MessageBuses;

namespace MadWorldNL.MantaRayPlan.Web.Services.MessageBuses;

public interface IMessageBusService
{
    GetStatusResponse GetStatus();
    PostStatusResponse PostNewStatus();
}