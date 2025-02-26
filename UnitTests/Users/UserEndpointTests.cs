﻿using Application.Users;
using System.Net.Http.Json;
using Application.Users.Register;
using Shouldly;
using Web.Api;

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
            "Password123!"
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
            "Password123!"
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
            "Password123!"
        );

        HttpResponseMessage response = await _client.PostAsJsonAsync("users/register", command);
        ErrorResponse? result = await response.Content.ReadFromJsonAsync<ErrorResponse>();

        response.IsSuccessStatusCode.ShouldBeFalse();
        Assert.Equal("UsernameTaken", result?.Code);
    }
}
