// See https://aka.ms/new-console-template for more information

using MadWorldNL.MantaRayPlan;
using MadWorldNL.MantaRayPlan.Extensions;
using MadWorldNL.MantaRayPlan.OpenTelemetry;

var builder = WebApplication.CreateBuilder();

var openTelemetryConfig = builder.Configuration.GetSection(OpenTelemetryConfig.Key).Get<OpenTelemetryConfig>() ??
                            new OpenTelemetryConfig();

builder.AddDefaultOpenTelemetry(openTelemetryConfig);

builder.AddDatabase();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/healthz");

app.Services.MigrateDatabase<MantaRayPlanDbContext>();

await app.RunAsync();

public abstract partial class Program { }