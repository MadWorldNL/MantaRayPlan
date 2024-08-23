using Microsoft.EntityFrameworkCore;

namespace MadWorldNL.MantaRayPlan.MessageBuses;

public class MessageBusRepository(MantaRayPlanDbContext dbContext) : IMessageBusRepository
{
    public async Task CreateAsync(MessageBusStatus status)
    {
        await dbContext.MessageBusStatus
                .AddAsync(status);
        
        await dbContext.SaveChangesAsync();
    }

    public async Task<MessageBusStatus?> FindStatusAsync()
    {
        return await dbContext.MessageBusStatus
                .OrderBy(s => s.Id)
                .LastOrDefaultAsync();
    }

    public async Task UpdateAsync(MessageBusStatus status)
    {
        dbContext.MessageBusStatus
            .Update(status);
        
        await dbContext.SaveChangesAsync();
    }
}