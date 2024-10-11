using Google.Protobuf.WellKnownTypes;
using MadWorldNL.MantaRayPlan.Api;
using MadWorldNL.MantaRayPlan.Base;
using MadWorldNL.MantaRayPlan.MessageBuses;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace MadWorldNL.MantaRayPlan.Services;

[Collection(TestDefinitions.Default)]
public class MessageBusServiceProxyTests(GrpcFactory factory)
{
    [Fact]
    public async Task GetStatus_WhenGivenEmptyRequest_ShouldReturnValidCounter()
    {
        // Arrange
        const int counter = 10;
        
        var messageBusClient = new MessageBusService.MessageBusServiceClient(factory.Channel);
        var serviceProvider = factory.GetServiceProvider().ServiceProvider;
        
        var dbContext = serviceProvider.GetRequiredService<MantaRayPlanDbContext>();
        var newStatus = new MessageBusStatus()
        {
            Id = 1,
            Count = counter
        };
        dbContext.MessageBusStatus.Add(newStatus);
        await dbContext.SaveChangesAsync();
        
        // Act
        var status = await messageBusClient.GetStatusAsync(new Empty());

        // Assert
        status.Counter.ShouldBe(counter);
        status.Message.ShouldBe(string.Empty);
    }

    [Fact]
    public async Task PostStatus_WhenGivenEmptyRequest_ShouldIncreaseCounterValue()
    {
        // Arrange
        var messageBusClient = new MessageBusService.MessageBusServiceClient(factory.Channel);
        
        var serviceProvider = factory.GetServiceProvider().ServiceProvider;
        
        // Act
        var response = await messageBusClient.PostStatusAsync(new Empty());

        // Assert
        response.Message.ShouldBe("");
        
        var harness = serviceProvider.GetRequiredService<ITestHarness>();
        await harness.Sent.Any<MessageBusStatusCommand>();
    }
}