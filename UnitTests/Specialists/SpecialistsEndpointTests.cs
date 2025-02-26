﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Specialists;
using Application.Specialists.Create;
using Application.Users.Login;
using Infrastructure.Authentication;
using Shouldly;
using Web.Api;

namespace Tests.Specialists;

[Collection("Factory")]
public sealed class SpecialistsEndpointTests(CustomWebApplicationFactory<Program> factory)
{
    private readonly HttpClient _client = factory.CreateClient();

    private async Task<string> GenerateSpecialistToken()
    {
        var command = new LoginUserCommand(SeedData.TestUsername, SeedData.TestPassword);
        HttpResponseMessage response = await _client.PostAsJsonAsync("users/login", command);
        string result = await response.Content.ReadAsStringAsync();
        TokenResponse? json = JsonSerializer.Deserialize<TokenResponse>(result);
        return json!.Token;
    }

    private class TokenResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; init; } = string.Empty;
    }

    [Fact]
    public async Task GetSpecialistsEndpoint_ShouldReturnSpecialists()
    {
        HttpResponseMessage response = await _client.GetAsync("specialists");
        List<SpecialistsResponse>? result = await response.Content.ReadFromJsonAsync <List<SpecialistsResponse>>();

        response.IsSuccessStatusCode.ShouldBeTrue();
        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task CreateSpecialistsEndpoint_ShouldReturnOk()
    {
        var command = new CreateSpecialistCommand(
            SeedData.TestUserId,
            SeedData.TestSpecializationName,
            "123456789",
            "Test Description",
            "Warsaw"
        );

        var request = new HttpRequestMessage(HttpMethod.Post, "specialists")
        {
            Content = JsonContent.Create(command)
        };
        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.SendAsync(request);
        response.IsSuccessStatusCode.ShouldBeTrue();
    }
}
