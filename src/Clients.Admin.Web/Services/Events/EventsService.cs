using MadWorldNL.MantaRayPlan.Events;
using MadWorldNL.MantaRayPlan.Web.Configurations;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;

namespace MadWorldNL.MantaRayPlan.Web.Services.Events;

public class EventsService : IAsyncDisposable
{
    private bool _isStarted;
    private readonly HubConnection _hubConnection;
    
    public event Action<MessageBusStatusEvent>? EventReceived;

    public EventsService(IOptions<ApiSettings> settings)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{settings.Value.Address}Events")
            .Build();
        
        _hubConnection.On<MessageBusStatusEvent>("NewEvent", (@event) =>
        {
            EventReceived?.Invoke(@event);
        });
    }

    public Task StartAsync()
    {
        if (_isStarted)
        {
            return Task.CompletedTask;
        }
        
        _isStarted = true;
        return _hubConnection.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await _hubConnection.StopAsync();
        await _hubConnection.DisposeAsync();
        
        GC.SuppressFinalize(this);
    }
}