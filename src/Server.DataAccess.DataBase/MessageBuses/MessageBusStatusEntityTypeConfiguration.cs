using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MadWorldNL.MantaRayPlan.MessageBuses;

public class MessageBusStatusEntityTypeConfiguration  : IEntityTypeConfiguration<MessageBusStatus>
{
    public void Configure(EntityTypeBuilder<MessageBusStatus> builder)
    {
        builder.HasKey(c => c.Id);
    }
}