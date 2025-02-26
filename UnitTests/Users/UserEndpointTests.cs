using Application.Users;
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
            "Client"
        );

        HttpResponseMessage response = await _client.PostAsJsonAsync("users/register", command);

        response.IsSuccessStatusCode.ShouldBeTrue();
    }

    [Fact]
    public async Task RegisterEndpoint_ShouldReturnConflict_EmailAlreadyExist()
    {
        var command = new RegisterUserCommand(
            "EndpointTest@example.com",
            "EndpointSecondTest",
            "Endpoint",
            "Second",
            "Password123!",
            "Client"
        );

        HttpResponseMessage response = await _client.PostAsJsonAsync("users/register", command);
        ErrorResponse? result = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        response.IsSuccessStatusCode.ShouldBeFalse();
        Assert.Equal("EmailTaken", result?.Code);
    }

    [Fact]
    public async Task RegisterEndpoint_ShouldReturnConflict_UsernameAlreadyExist()
    {
        var command = new RegisterUserCommand(
            "EndpointThirdTest@example.com",
            "EndpointTest",
            "Endpoint",
            "Third",
            "Password123!",
            "Client"
        );

        HttpResponseMessage response = await _client.PostAsJsonAsync("users/register", command);
        ErrorResponse? result = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        response.IsSuccessStatusCode.ShouldBeFalse();
        Assert.Equal("UsernameTaken", result?.Code);
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
        result.ShouldNotBeNullOrWhiteSpace();
    }


    [Fact]
    public async Task LoginEndpoint_ShouldReturnNotFound_WrongUsername()
    {
        var command = new LoginUserCommand(
            "EndpointTestNotExist@example.com",
            "Password123!"
        );

        HttpResponseMessage response = await _client.PostAsJsonAsync("users/login", command);
        ErrorResponse? result = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        response.IsSuccessStatusCode.ShouldBeFalse();
        Assert.Equal("UserNotFound", result?.Code);
    }

    [Fact]
    public async Task LoginEndpoint_ShouldReturnUnauthorized_WrongPassword()
    {
        var command = new LoginUserCommand(
            "EndpointTest@example.com",
            "Passwordddddddddd123!"
        );

        HttpResponseMessage response = await _client.PostAsJsonAsync("users/login", command);

        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
