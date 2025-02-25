using Application.Users;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Shouldly;

namespace Tests.Users;

public class UserEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public UserEndpointTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task RegisterEndpoint_ShouldReturnOk_WhenValidData()
    {
        HttpClient client = _factory.CreateClient();
        var command = new RegisterUserCommand(
            "EndpointFirstTest@example.com",
            "EndpointFirstTest",
            "Endpoint",
            "Test",
            "Password123!"
        );

        HttpResponseMessage response = await client.PostAsJsonAsync("users/register", command);

        response.IsSuccessStatusCode.ShouldBeTrue();
    }

}
