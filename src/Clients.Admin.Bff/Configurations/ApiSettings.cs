using System.ComponentModel.DataAnnotations;

namespace MadWorldNL.MantaRayPlan.Configurations;

public class ApiSettings
{
    public const string Key = "Api";

    [Required]
    public string Address { get; init; } = string.Empty;
}