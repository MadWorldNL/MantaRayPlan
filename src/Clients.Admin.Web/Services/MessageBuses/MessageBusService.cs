using System.Net.Http.Json;
using MadWorldNL.MantaRayPlan.MessageBuses;
using MadWorldNL.MantaRayPlan.Web.Configurations;

namespace MadWorldNL.MantaRayPlan.Web.Services.MessageBuses;

public class MessageBusService(IHttpClientFactory clientFactory) : IMessageBusService
{
    private const string EndpointSingular = "MessageBus";
    
    private readonly HttpClient _client = clientFactory.CreateClient(ApiTypes.AdminBff);

    public async Task<GetStatusResponse> GetStatusAsync()
    {
        const int emptyCounter = -1;
        
        return await _client.GetFromJsonAsync<GetStatusResponse>($"{EndpointSingular}/Status") ?? new GetStatusResponse("No response", emptyCounter);
    }

    public async Task<PostStatusResponse> PostNewStatusAsync()
    {
        return new PostStatusResponse("");
    }
}