using System.Net.Http.Json;
using Application.Users.Login;
using Application.Users.Register;
using Shouldly;
using Web.Api;
using System.Net;

namespace Tests.Users;

[Collection("Factory")]
public sealed class UserEndpointTests(CustomWebApplicationFactory<Program> factory)
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task RegisterEndpoint_ShouldReturnOk()
    {
        var command = new RegisterUserCommand(
            "EndpointFirstTest@example.com",
            "EndpointFirstTest",
            "Endpoint",
            "Test",
            "Password123!",
            SeedData.TestRoleId
        );

        HttpResponseMessage response = await _client.PostAsJsonAsync("users/register", command);

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task LoginEndpoint_ShouldReturnOk()
    {
        var command = new LoginUserCommand(
            "EndpointTest@example.com",
            "Password123!"
        );

        HttpResponseMessage response = await _client.PostAsJsonAsync("users/login", command);
        string result = await response.Content.ReadAsStringAsync();

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNullOrWhiteSpace();
    }
}
