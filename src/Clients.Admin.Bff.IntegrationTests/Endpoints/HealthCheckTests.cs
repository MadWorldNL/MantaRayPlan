using System.Net;
using MadWorldNL.MantaRayPlan.Base;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace MadWorldNL.MantaRayPlan.Endpoints;

[Collection(TestDefinitions.Default)]
public class HealthCheckTests(AdminBffFactory factory)
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