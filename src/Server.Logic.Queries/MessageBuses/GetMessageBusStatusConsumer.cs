using MassTransit;

namespace MadWorldNL.MantaRayPlan.MessageBuses;

public class GetMessageBusStatusConsumer : IConsumer<GetMessageBusStatus>
{
    private readonly IMessageBusRepository _messageBusRepository;

    public GetMessageBusStatusConsumer(IMessageBusRepository messageBusRepository)
    {
        _messageBusRepository = messageBusRepository;
    }
    
    
    public async Task Consume(ConsumeContext<GetMessageBusStatus> context)
    {
        var messageBusStatus = await _messageBusRepository.FindStatusAsync() ?? new MessageBusStatus();
        await context.RespondAsync(messageBusStatus);
    }
}