using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;

namespace MadWorldNL.MantaRayPlan.Base;

public class MessageBusFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private const string DbName = "MantaRayPlan";
    private const string DbUser = "postgres";
    private const string DbPassword = "Password1234!";

    private const string BusUser = "development";
    private const string BusPassword = "Password1234";
    
    private PostgreSqlContainer? _postgreSqlContainer;
    private RabbitMqContainer? _rabbitMqContainer;
    
    public async Task InitializeAsync()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithDatabase(DbName)
            .WithUsername(DbUser)
            .WithPassword(DbPassword)
            .Build();

        _rabbitMqContainer = new RabbitMqBuilder()
            .WithImage("rabbitmq:3.11")
            .WithUsername(BusUser)
            .WithPassword(BusPassword)
            .Build();

        await _postgreSqlContainer.StartAsync();
        await _rabbitMqContainer.StartAsync();
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var newSettings = new Dictionary<string, string>
        {
            ["Database:Host"] = _postgreSqlContainer!.Hostname,
            ["Database:Port"] = _postgreSqlContainer.GetMappedPublicPort(5432).ToString(),
            ["Database:DbName"] = DbName,
            ["Database:User"] = DbUser,
            ["Database:Password"] = DbPassword,
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
        if (_postgreSqlContainer is not null)
        {
            await _postgreSqlContainer.DisposeAsync();
        }
        
        if (_rabbitMqContainer is not null)
        {
            await _rabbitMqContainer.DisposeAsync();
        }
        
        await base.DisposeAsync();
    }
}