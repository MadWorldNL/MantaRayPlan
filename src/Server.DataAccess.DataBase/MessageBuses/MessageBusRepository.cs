using Microsoft.EntityFrameworkCore;

namespace MadWorldNL.MantaRayPlan.MessageBuses;

public class MessageBusRepository : IMessageBusRepository
{
    private readonly MantaRayPlanDbContext _dbContext;

    public MessageBusRepository(MantaRayPlanDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(MessageBusStatus status)
    {
        await _dbContext.MessageBusStatus
                .AddAsync(status);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task<MessageBusStatus?> FindStatusAsync()
    {
        return await _dbContext.MessageBusStatus
                .LastOrDefaultAsync();
    }

    public async Task UpdateAsync(MessageBusStatus status)
    {
        _dbContext.MessageBusStatus
            .Update(status);
        
        await _dbContext.SaveChangesAsync();
    }
}