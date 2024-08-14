namespace MadWorldNL.MantaRayPlan.Configurations;

public class OpenTelemetryConfig
{
    public const string Key = "OpenTelemetry";

    public string LoggerEndpoint { get; set; } = "http://localhost:5341";
    public string LoggerApiKey { get; set; } = "X-Seq-ApiKey=abcde12345";
}