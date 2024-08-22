using MadWorldNL.MantaRayPlan.MessageBuses;
using Microsoft.EntityFrameworkCore;

namespace MadWorldNL.MantaRayPlan;

public class MantaRayPlanDbContext : DbContext
{
    public MantaRayPlanDbContext(DbContextOptions<MantaRayPlanDbContext> options) : base(options)
    {
    }
    
    public DbSet<MessageBusStatus> MessageBusStatus { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration( new MessageBusStatusEntityTypeConfiguration());
    }
}