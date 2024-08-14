// See https://aka.ms/new-console-template for more information

using MadWorldNL.MantaRayPlan.OpenTelemetry;

var builder = WebApplication.CreateBuilder();

var openTelemetryConfig = builder.Configuration.GetSection(OpenTelemetryConfig.Key).Get<OpenTelemetryConfig>() ??
                          new OpenTelemetryConfig();

builder.Services.AddDefaultOpenTelemetry(openTelemetryConfig);

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/healthz");

await app.RunAsync();

public abstract partial class Program { }