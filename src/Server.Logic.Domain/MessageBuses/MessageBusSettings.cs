namespace MadWorldNL.MantaRayPlan.MessageBuses;

public class MessageBusSettings
{
    public const string Key = "MessageBus";
    
    public string Host { get; init; } = string.Empty;
    public ushort Port { get; init; }
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}