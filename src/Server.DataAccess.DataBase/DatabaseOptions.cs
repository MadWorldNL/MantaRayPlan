namespace MadWorldNL.MantaRayPlan;

public class DatabaseOptions
{
    public const string Key = "Database";
    
    public string Host { get; init; } = string.Empty;
    public int Port { get; init; }
    public string User { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string DbName { get; init; } = string.Empty;
    
    public string ConnectionString => $"Host={Host};Port={Port};Username={User};Password={Password};Database={DbName};";
}