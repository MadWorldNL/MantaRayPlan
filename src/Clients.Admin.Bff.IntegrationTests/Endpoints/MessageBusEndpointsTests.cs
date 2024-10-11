using System.Net.Http.Json;
using Google.Protobuf.WellKnownTypes;
using MadWorldNL.MantaRayPlan.Api;
using MadWorldNL.MantaRayPlan.Base;
using MadWorldNL.MantaRayPlan.MessageBuses;
using NSubstitute;
using Shouldly;

namespace MadWorldNL.MantaRayPlan.Endpoints;

[Collection(TestDefinitions.Default)]
public class MessageBusEndpointsTests(AdminBffFactory factory)
{
    [Fact]
    public async Task GetStatus_WhenSendWithoutParameters_ShouldReturnCurrentCounter()
    {
        // Arrange
        factory.MessageBusServiceClient.GetStatus(new Empty()).Returns(new GetMessageBusStatusReply()
        {
            Counter = 1,
            Message = ""
        });
        
        var client = factory.CreateClient();
        
        // Act
        var response  = await client.GetFromJsonAsync<GetStatusResponse>("/MessageBus/Status");
        
        // Assert
        response.ShouldNotBeNull();
        response.message.ShouldBe(string.Empty);
        response.counter.ShouldBe(1);
    }
    
    [Fact]
    public async Task PostStatus_WhenSendWithoutParameters_ShouldIncreaseCounter()
    {
        // Arrange
        factory.MessageBusServiceClient.PostStatus(new Empty()).Returns(new PostMessageBusStatusReply()
        {
            Message = ""
        });
        
        var client = factory.CreateClient();
        
        // Act
        var httpResponse  = await client.PostAsJsonAsync<string>("/MessageBus/Status", "RandomData");
        
        // Assert
        httpResponse.ShouldNotBeNull();
        httpResponse.EnsureSuccessStatusCode(); 
        
        var response = await httpResponse.Content.ReadFromJsonAsync<PostStatusResponse>();
        response.ShouldNotBeNull();
        response.message.ShouldBe(string.Empty);
    }
}