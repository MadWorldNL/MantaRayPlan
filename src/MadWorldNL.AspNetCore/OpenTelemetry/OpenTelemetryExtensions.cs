using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace MadWorldNL.MantaRayPlan.OpenTelemetry;

public static class OpenTelemetryExtensions
{
    public static void AddDefaultOpenTelemetry(this IServiceCollection services, OpenTelemetryConfig config)
    {
        services.AddOpenTelemetry()
            .WithLogging(loggerBuilder =>
            {
                loggerBuilder.ConfigureResource(builder =>
                {
                    builder.AddService(config.Application);
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