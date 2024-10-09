using Google.Protobuf.WellKnownTypes;
using MadWorldNL.MantaRayPlan.Api;
using MadWorldNL.MantaRayPlan.Base;
using MadWorldNL.MantaRayPlan.Events;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace MadWorldNL.MantaRayPlan.Services;

[Collection(TestDefinitions.Default)]
public class EventServiceProxyTests(GrpcFactory factory)
{
    [Fact]
    public async Task Subscribe_WhenGivenNewEvent_ShouldReceiveEvent()
    {
        // Arrange
        var eventClient = new EventService.EventServiceClient(factory.Channel);
        var serviceProvider = factory.GetServiceProvider().ServiceProvider;
        var harness = serviceProvider.GetRequiredService<ITestHarness>();

        // Act
        var responses = eventClient.Subscribe(new Empty());
        await harness.Bus.Publish(new MessageBusStatusEvent
        {
            Count = 10
        });

        // Assert
        await responses.ResponseStream.MoveNext(CancellationToken.None);
        var @event = responses.ResponseStream.Current;
        @event.Type.ShouldBe(nameof(MessageBusStatusEvent));
        @event.Json.ShouldBe("{\"Count\":10}");
    }
}