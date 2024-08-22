namespace MadWorldNL.MantaRayPlan.Application;

public class OptionNotFoundException : Exception
{
    public readonly string Key;
    public OptionNotFoundException(string key)
    {
        Key = key;
    }
}