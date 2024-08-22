namespace MadWorldNL.MantaRayPlan.MessageBuses;

public class MessageBusStatus
{
    public int Id { get; private set; }
    public int Count { get; private set; }
    
    public void IncrementCount()
    {
        Count++;
    }
}