using MadWorldNL.MantaRayPlan.Api;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using NSubstitute;
using Testcontainers.RabbitMq;

namespace MadWorldNL.MantaRayPlan.Base;

public class AdminBffFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public MessageBusService.MessageBusServiceClient MessageBusServiceClient = Substitute.For<MessageBusService.MessageBusServiceClient>();
    
    private const string BusUser = "development";
    private const string BusPassword = "Password1234";
    
    private RabbitMqContainer? _rabbitMqContainer;
    
    public async Task InitializeAsync()
    {
        _rabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3.11")
            .WithUsername(BusUser)
            .WithPassword(BusPassword)
            .Build();
        
        await _rabbitMqContainer.StartAsync();
    }
    
    public IServiceScope GetServiceProvider()
    {
        return Services.GetService<IServiceScopeFactory>()!.CreateScope();
    }

    public async Task<HubConnection> CreateSignalRClientAsync<TReceiveType>(string hubName, string methodeName, Action<TReceiveType> handler)
    {
        var connection = new HubConnectionBuilder()
            .WithUrl(
                $"{Server.BaseAddress}{hubName}",
                o => o.HttpMessageHandlerFactory = _ => Server.CreateHandler())
            .Build();
        
        connection.On(methodeName, handler);
        
        await connection.StartAsync();

        return connection;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var newSettings = new Dictionary<string, string>
        {
            ["MessageBus:Host"] = _rabbitMqContainer!.Hostname,
            ["MessageBus:Port"] = _rabbitMqContainer.GetMappedPublicPort(5672).ToString(),
            ["MessageBus:Username"] = BusUser,
            ["MessageBus:Password"] = BusPassword
        };
        
        builder.ConfigureHostConfiguration(config =>
        {
            config.AddInMemoryCollection(newSettings!);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<MessageBusService.MessageBusServiceClient>();
            services.AddScoped<MessageBusService.MessageBusServiceClient>(_ => MessageBusServiceClient);
            
            // For more info about testing message bus:
            // https://masstransit.io/documentation/concepts/testing
            services.AddMassTransitTestHarness();
        });
        
        return base.CreateHost(builder);
    }

    public new async Task DisposeAsync()
    {
        if (_rabbitMqContainer is not null)
        {
            await _rabbitMqContainer.DisposeAsync();
        }
        
        await base.DisposeAsync();
    }
}