namespace MadWorldNL.MantaRayPlan.MessageBuses;

public interface IMessageBusRepository
{
    Task CreateAsync(MessageBusStatus status);
    Task<MessageBusStatus?> FindStatusAsync();
    Task UpdateAsync(MessageBusStatus status);
}