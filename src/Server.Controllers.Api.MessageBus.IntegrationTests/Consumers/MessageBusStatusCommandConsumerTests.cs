using MadWorldNL.MantaRayPlan.Base;
using MadWorldNL.MantaRayPlan.MessageBuses;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace MadWorldNL.MantaRayPlan.Consumers;

[Collection(TestDefinitions.Default)]
public class MessageBusStatusCommandConsumerTests(MessageBusFactory factory)
{
    [Fact]
    public async Task Consume_WhenNewStatusIsRequest_ShouldIncreaseCounter()
    {
        // Arrange
        var serviceProvider = factory.GetServiceProvider().ServiceProvider;
        var harness = serviceProvider.GetRequiredService<ITestHarness>();
        var consumer = await harness.GetConsumerEndpoint<MessageBusStatusCommandConsumer>();
        
        // Act
        await consumer.Wait(c => c.Send(new MessageBusStatusCommand("TestData")), TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));

        // Assert
        var context = serviceProvider.GetRequiredService<MantaRayPlanDbContext>();
        var statuses = context.MessageBusStatus.ToList();
        statuses.Count.ShouldBe(1);
        statuses[0].ShouldSatisfyAllConditions(s =>
            {
                s.Id.ShouldBe(1);
                s.Count.ShouldBe(1);
            });
    }
}