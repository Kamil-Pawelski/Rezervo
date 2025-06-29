using System.Net.Http.Json;
using Application.Users.Login;
using Application.Users.Register;
using Shouldly;
using System.Net;
using Tests.IntegrationTestsConfiguration;

namespace Tests.Users;

[Collection("Factory")]
public sealed class UserEndpointTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{

    [Fact]
    public async Task RegisterEndpoint_ShouldReturnOk()
    {
        var command = new RegisterUserCommand(
            "EndpointFirstTest@example.com",
            "EndpointFirstTest",
            "Endpoint",
            "Test",
            "Password123!",
            SeedUser.TestRoleId
        );

        HttpResponseMessage response = await Client.PostAsJsonAsync("users/register", command);

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

        HttpResponseMessage response = await Client.PostAsJsonAsync("users/login", command);
        string result = await response.Content.ReadAsStringAsync();

        response.IsSuccessStatusCode.ShouldBeTrue();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.ShouldNotBeNullOrWhiteSpace();
    }
}
