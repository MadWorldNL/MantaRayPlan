// See https://aka.ms/new-console-template for more information

using MadWorldNL.MantaRayPlan.Extensions;
using MadWorldNL.MantaRayPlan.MassTransit;
using MadWorldNL.MantaRayPlan.MessageBuses;
using MadWorldNL.MantaRayPlan.OpenTelemetry;
using MassTransit;

namespace MadWorldNL.MantaRayPlan;

public class Program()
{
    private const string CorsName = "DefaultCors";
    
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: CorsName,
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

        builder.AddDatabase();

        builder.Services.AddHealthChecks();

        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
    
            x.AddConsumer<MessageBusStatusCommandConsumer>();
    
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

        app.Run();
    }
}