using MadWorldNL.MantaRayPlan.Base;
using MadWorldNL.MantaRayPlan.Events;
using MassTransit.Internals;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace MadWorldNL.MantaRayPlan.Hubs;

[Collection(TestDefinitions.Default)]
public class EventsHubTests(AdminBffFactory factory)
{
    [Fact]
    public async Task NewEvent_WhenGivenNewEvent_ShouldReceiveEvent()
    {
        // Arrange
        var serviceProvider = factory.GetServiceProvider().ServiceProvider;
        var harness = serviceProvider.GetRequiredService<ITestHarness>();
        
        var source = new TaskCompletionSource<MessageBusStatusEvent>();
        await factory.CreateSignalRClientAsync<MessageBusStatusEvent>("Events",
            "NewEvent", @event =>
            {
                source.TrySetResult(@event);
            });
            
        // Act
        await harness.Bus.Publish(new MessageBusStatusEvent()
        {
            Count = 10
        });
        
        var statusEvent = await source.Task.OrTimeout(TimeSpan.FromSeconds(3));
        
        // Assert
        statusEvent.ShouldNotBeNull();
        statusEvent.Count.ShouldBe(10);
    }
}