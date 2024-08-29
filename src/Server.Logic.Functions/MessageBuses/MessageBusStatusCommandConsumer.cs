using MassTransit;

namespace MadWorldNL.MantaRayPlan.MessageBuses;

public class MessageBusStatusCommandConsumer : IConsumer<MessageBusStatusCommand>
{
    private readonly IMessageBusRepository _messageBusRepository;

    public MessageBusStatusCommandConsumer(IMessageBusRepository messageBusRepository)
    {
        _messageBusRepository = messageBusRepository;
    }
    
    public async Task Consume(ConsumeContext<MessageBusStatusCommand> context)
    {
        var status = await _messageBusRepository.FindStatusAsync();

        if (status is null)
        {
            status = new MessageBusStatus();
            
            await _messageBusRepository.CreateAsync(status);
        }
        
        status.IncrementCount();
        
        await _messageBusRepository.UpdateAsync(status);

        await context.Publish(new MessageBusStatusEvent
        {
            Count = status.Count
        });
    }
}