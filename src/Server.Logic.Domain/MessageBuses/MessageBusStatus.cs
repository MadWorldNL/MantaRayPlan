namespace MadWorldNL.MantaRayPlan.MessageBuses;

public class MessageBusStatus
{
    public int Id { get; set; }
    public int Count { get; set; }
    
    public void IncrementCount()
    {
        Count++;
    }
}