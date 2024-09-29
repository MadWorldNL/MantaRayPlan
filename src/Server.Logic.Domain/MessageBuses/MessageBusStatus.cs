namespace MadWorldNL.MantaRayPlan.MessageBuses;

public class MessageBusStatus
{
    public int Id { get; init; }
    public int Count { get; set; }
    
    public void IncrementCount()
    {
        if (Count >= int.MaxValue)
        {
            Count = 0;
        }
        
        Count++;
    }
}