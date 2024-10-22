using MadWorldNL.MantaRayPlan.Events;
using MadWorldNL.MantaRayPlan.Extensions;
using MadWorldNL.MantaRayPlan.MassTransit;
using MadWorldNL.MantaRayPlan.MessageBuses;
using MadWorldNL.MantaRayPlan.OpenTelemetry;
using MadWorldNL.MantaRayPlan.Services;
using MassTransit;

const string corsName = "DefaultCors";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsName,
        policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });
});

var openTelemetryConfig = builder.Configuration.GetSection(OpenTelemetryConfig.Key).Get<OpenTelemetryConfig>() 
                            ?? new OpenTelemetryConfig();

builder.AddDefaultOpenTelemetry(openTelemetryConfig);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.AddDatabase();

builder.Services.AddHealthChecks();

EndpointConvention.Map<MessageBusStatusCommand>(new Uri(GetExchangeName<MessageBusStatusCommand>()));
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumer<EventPusherConsumer<MessageBusStatusEvent>>();
    
    x.AddConsumer<MessageBusStatusQueryConsumer>()
        .Endpoint(e => e.Name = GetName<MessageBusStatusQuery>());
    
    x.AddRequestClient<MessageBusStatusQuery>(new Uri(GetExchangeName<MessageBusStatusQuery>()));
    
    x.UsingRabbitMq((context,cfg) =>
    {
        var messageBusSettings  = builder.Configuration.GetSection(MessageBusSettings.Key)
            .Get<MessageBusSettings>()!;
        
        cfg.Host(messageBusSettings.Host, messageBusSettings.Port, "/", h => {
            h.Username(messageBusSettings.Username);
            h.Password(messageBusSettings.Password);
        });
        
        var appName = typeof(Program).Assembly.GetName().Name!;
        cfg.ReceiveEndpoint(EventPusherConsumer<MessageBusStatusEvent>.GetQueueName(appName, nameof(MessageBusStatusEvent)), 
            e =>
            {
                e.ConfigureConsumer<EventPusherConsumer<MessageBusStatusEvent>>(context);
            });
        
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();
app.UseCors(corsName);

app.MapGrpcService<EventServiceProxy>();
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

public abstract partial class Program
{
    public static string GetExchangeName<T>() => $"exchange:{GetName<T>()}";
    private static string GetName<T>() => $"{typeof(T).Namespace}:{typeof(T).Name}";
}