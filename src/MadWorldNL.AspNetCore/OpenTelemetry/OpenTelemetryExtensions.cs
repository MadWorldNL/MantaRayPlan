using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace MadWorldNL.MantaRayPlan.OpenTelemetry;

public static class OpenTelemetryExtensions
{   
    public static void AddDefaultOpenTelemetry(this WebApplicationBuilder builder, OpenTelemetryConfig config)
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });
        
        builder.Services.AddOpenTelemetry()
            .WithLogging(loggerBuilder =>
            {
                loggerBuilder.ConfigureResource(resourceBuilder =>
                {
                    resourceBuilder.AddService(config.Application);
                });
                
                loggerBuilder.AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri($"{config.LoggerEndpoint}/ingest/otlp/v1/logs");
                    options.Protocol = OtlpExportProtocol.HttpProtobuf;
            
                    if (!string.IsNullOrEmpty(config.LoggerApiKey))
                    {
                        options.Headers = $"X-Seq-ApiKey={config.LoggerApiKey}";
                    }
                });
            });
    }
}