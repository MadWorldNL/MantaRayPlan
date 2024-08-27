// See https://aka.ms/new-console-template for more information

using MadWorldNL.MantaRayPlan;
using MadWorldNL.MantaRayPlan.Extensions;
using MadWorldNL.MantaRayPlan.MessageBuses;
using MadWorldNL.MantaRayPlan.OpenTelemetry;
using MassTransit;

var builder = WebApplication.CreateBuilder();

var openTelemetryConfig = builder.Configuration.GetSection(OpenTelemetryConfig.Key).Get<OpenTelemetryConfig>() ??
                            new OpenTelemetryConfig();

builder.AddDefaultOpenTelemetry(openTelemetryConfig);

builder.AddDatabase();

builder.Services.AddHealthChecks();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    
    x.UsingRabbitMq((context,cfg) =>
    {
        var messageBusSettings  = builder.Configuration.GetSection(MessageBusSettings.Key)
            .Get<MessageBusSettings>()!;
        
        cfg.Host(messageBusSettings.Host, messageBusSettings.Port, "/", h => {
            h.Username(messageBusSettings.Username);
            h.Password(messageBusSettings.Password);
        });
        
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.MapHealthChecks("/healthz");

app.Services.MigrateDatabase<MantaRayPlanDbContext>();

await app.RunAsync();

public abstract partial class Program { }