using MadWorldNL.MantaRayPlan.Application;
using MadWorldNL.MantaRayPlan.MessageBuses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace MadWorldNL.MantaRayPlan.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static void AddDatabase(this IHostApplicationBuilder builder)
    {
        var databaseOptions = builder.Configuration
                                    .GetSection(DatabaseOptions.Key)
                                    .Get<DatabaseOptions>() ?? throw new OptionNotFoundException(DatabaseOptions.Key);
        
        builder.Services.AddDbContext<MantaRayPlanDbContext>(options =>
            options.UseNpgsql(
                databaseOptions.ConnectionString,
                b => b.MigrationsAssembly("MadWorldNL.MantaRayPlan.DataAccess.Database")));
        
        builder.Services.AddScoped<IMessageBusRepository, MessageBusRepository>();
    }
}