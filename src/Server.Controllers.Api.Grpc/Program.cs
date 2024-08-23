using MadWorldNL.MantaRayPlan.Extensions;
using MadWorldNL.MantaRayPlan.OpenTelemetry;
using MadWorldNL.MantaRayPlan.Services;

var builder = WebApplication.CreateBuilder(args);

var openTelemetryConfig = builder.Configuration.GetSection(OpenTelemetryConfig.Key).Get<OpenTelemetryConfig>() 
                            ?? new OpenTelemetryConfig();

builder.AddDefaultOpenTelemetry(openTelemetryConfig);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

builder.AddDatabase();

builder.Services.AddHealthChecks();

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