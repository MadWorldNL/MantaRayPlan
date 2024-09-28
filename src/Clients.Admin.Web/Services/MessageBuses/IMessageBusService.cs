using MadWorldNL.MantaRayPlan.MessageBuses;

namespace MadWorldNL.MantaRayPlan.Web.Services.MessageBuses;

public interface IMessageBusService
{
    Task<GetStatusResponse> GetStatusAsync();
    Task<PostStatusResponse> PostNewStatusAsync();
}