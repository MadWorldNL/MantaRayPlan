
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace MadWorldNL.MantaRayPlan;

public class HealthCheckTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
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
}