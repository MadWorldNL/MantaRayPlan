using MadWorldNL.MantaRayPlan.Extensions;
using MadWorldNL.MantaRayPlan.MessageBuses;
using MadWorldNL.MantaRayPlan.OpenTelemetry;
using MadWorldNL.MantaRayPlan.Services;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

var openTelemetryConfig = builder.Configuration.GetSection(OpenTelemetryConfig.Key).Get<OpenTelemetryConfig>() 
                            ?? new OpenTelemetryConfig();

builder.AddDefaultOpenTelemetry(openTelemetryConfig);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.AddDatabase();

builder.Services.AddHealthChecks();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    
    x.AddConsumer<GetMessageBusStatusConsumer>()
        .Endpoint(e => e.Name = nameof(GetMessageBusStatus));
    
    x.AddRequestClient<GetMessageBusStatus>(new Uri($"exchange:{nameof(GetMessageBusStatus)}"));
    
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

app.MapGrpcService<MessageBusServiceProxy>();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapHealthChecks("/healthz");

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

await app.RunAsync();

public abstract partial class Program { }