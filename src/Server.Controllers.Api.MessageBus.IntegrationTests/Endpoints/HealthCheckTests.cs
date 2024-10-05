using System.Net;
using MadWorldNL.MantaRayPlan.Base;
using Shouldly;

namespace MadWorldNL.MantaRayPlan.Endpoints;

[Collection(TestDefinitions.Default)]
public class HealthCheckTests(MessageBusFactory factory) : IAsyncLifetime
{
    [Fact]
    public async Task Healthz_GivenEmptyRequest_ShouldBeHealthy()
    {
        // Arrange
        var client = factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/healthz");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => factory.DisposeAsync();
}