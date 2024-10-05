using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.RabbitMq;

namespace MadWorldNL.MantaRayPlan.Base;

public class AdminBffFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
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
        GC.SuppressFinalize(this);
    }
}