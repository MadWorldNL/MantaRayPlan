using System.Threading.RateLimiting;
using MadWorldNL.MantaRayPlan.Api;
using MadWorldNL.MantaRayPlan.Configurations;
using MadWorldNL.MantaRayPlan.Endpoints;
using MadWorldNL.MantaRayPlan.Events;
using MadWorldNL.MantaRayPlan.Hubs;
using MadWorldNL.MantaRayPlan.MassTransit;
using MadWorldNL.MantaRayPlan.OpenTelemetry;
using MassTransit;
using Microsoft.AspNetCore.RateLimiting;

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

var openTelemetryConfig = builder.Configuration.GetSection(OpenTelemetryConfig.Key).Get<OpenTelemetryConfig>() ??
                            new OpenTelemetryConfig();

builder.AddDefaultOpenTelemetry(openTelemetryConfig);

builder.Services.AddSignalR();
builder.Services.AddSingleton<EventHandlerService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSignalRSwaggerGen();
});

builder.Services.AddHealthChecks();

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumer<EventPusherConsumer<MessageBusStatusEvent>>();
    
    x.UsingRabbitMq((context,cfg) =>
    {
        var messageBusSettings  = builder.Configuration.GetSection(MessageBusSettings.Key).Get<MessageBusSettings>()!;
        
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

var apiSettingsSection = builder.Configuration.GetSection(ApiSettings.Key);
builder.Services.AddOptions<ApiSettings>()
    .Bind(apiSettingsSection)
    .ValidateDataAnnotations()
    .ValidateOnStart();
var apiSettings = apiSettingsSection.Get<ApiSettings>();

builder.Services.AddGrpcClient<MessageBusService.MessageBusServiceClient>(options =>
{
    options.Address = new Uri(apiSettings!.Address);
});

builder.Services.AddRateLimiter(rl => rl
    .AddFixedWindowLimiter(policyName: RateLimiterConfig.DefaultName, options =>
    {
        options.PermitLimit = 10;
        options.Window = TimeSpan.FromSeconds(10);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 5;
    }));

var app = builder.Build();

app.UseCors(corsName);
app.UseRateLimiter();

// Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Remove("Server");
    context.Response.Headers.Remove("X-Powered-By");
    context.Response.Headers.XFrameOptions = "DENY";
    context.Response.Headers.XContentTypeOptions = "nosniff";
    context.Response.Headers.ContentSecurityPolicy = "default-src 'self'; style-src 'self' 'unsafe-inline'; script-src 'self' 'unsafe-inline' 'unsafe-eval';";
    context.Response.Headers["Referrer-Policy"] = "no-referrer";
    context.Response.Headers["Permissions-Policy"] = "accelerometer=(), ambient-light-sensor=(), autoplay=(), battery=(), camera=(), cross-origin-isolated=(), display-capture=(), document-domain=(), encrypted-media=(), execution-while-not-rendered=(), execution-while-out-of-viewport=(), fullscreen=(), geolocation=(), gyroscope=(), keyboard-map=(), magnetometer=(), microphone=(), midi=(), navigation-override=(), payment=(), picture-in-picture=(), publickey-credentials-get=(), screen-wake-lock=(), sync-xhr=(), usb=(), web-share=(), xr-spatial-tracking=()";
    await next();
});

app.UseSwagger();
app.UseSwaggerUI();

app.MapHub<EventsHub>("/Events");

app.MapHealthChecks("/healthz");

var defaultEndpoints = app.MapGroup("/")
    .WithOpenApi()
    .RequireRateLimiting(RateLimiterConfig.DefaultName);

defaultEndpoints.AddMessageBusEndpoints();

await app.RunAsync();

public abstract partial class Program { }