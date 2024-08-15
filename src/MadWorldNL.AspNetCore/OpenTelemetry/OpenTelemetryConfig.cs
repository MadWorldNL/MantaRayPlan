namespace MadWorldNL.MantaRayPlan.OpenTelemetry;

public class OpenTelemetryConfig
{
    public const string Key = "OpenTelemetry";

    public string Application { get; init; } = string.Empty;
    public string LoggerApiKey { get; init; } = string.Empty;
    public string LoggerEndpoint { get; init; } = string.Empty;
}