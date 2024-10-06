using MassTransit;

namespace MadWorldNL.MantaRayPlan.MessageBuses;

public class MessageBusStatusQueryConsumer : IConsumer<MessageBusStatusQuery>
{
    private readonly IMessageBusRepository _messageBusRepository;

    public MessageBusStatusQueryConsumer(IMessageBusRepository messageBusRepository)
    {
        _messageBusRepository = messageBusRepository;
    }
    
    
    public async Task Consume(ConsumeContext<MessageBusStatusQuery> context)
    {
        var messageBusStatus = await _messageBusRepository.FindStatusAsync() ?? new MessageBusStatus();
        await context.RespondAsync(messageBusStatus);
    }
}