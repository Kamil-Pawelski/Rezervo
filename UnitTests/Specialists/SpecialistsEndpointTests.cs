using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Application.Specialists;
using Application.Specialists.Create;
using Application.Specialists.Put;
using Application.Users.Login;
using Shouldly;
using Tests.Response;
using Web.Api;

namespace Tests.Specialists;

[Collection("Factory")]
public sealed partial class SpecialistsEndpointTests(CustomWebApplicationFactory<Program> factory)
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

    private async Task<string> GenerateAdminToken()
    {
        var command = new LoginUserCommand(SeedData.TestAdminUsername, SeedData.TestPassword);
        HttpResponseMessage response = await _client.PostAsJsonAsync("users/login", command);
        string result = await response.Content.ReadAsStringAsync();
        TokenResponse? json = JsonSerializer.Deserialize<TokenResponse>(result);
        return json!.Token;
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
            SeedData.TestUserId2,
            SeedData.TestSpecializationId,
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

    [Fact]
    public async Task GetByIdSpecialistEndpoint_ShouldReturnSpecialist()
    {
        HttpResponseMessage response = await _client.GetAsync($"specialists/{SeedData.TestSpecialistId}");
        SpecialistsResponse? result = await response.Content.ReadFromJsonAsync<SpecialistsResponse>();
        response.IsSuccessStatusCode.ShouldBeTrue();
        result.ShouldNotBeNull();
        result!.Id.ShouldBe(SeedData.TestSpecialistId);
    }

    [Fact]
    public async Task GetByIdSpecialistEndpoint_ShouldReturnNotFound()
    {
        HttpResponseMessage response = await _client.GetAsync($"specialists/{Guid.NewGuid()}");
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PutSpecialistEndpoint_ShouldReturnOk()
    {
        var command = new PutSpecialistCommand(
            SeedData.TestSpecialistId,
            SeedData.TestUserId,
            "123456789",
            "Test Description",
            "London"
        );

        var request = new HttpRequestMessage(HttpMethod.Put, $"specialists/{SeedData.TestSpecialistId}")
        {
            Content = JsonContent.Create(command)
        };

        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.SendAsync(request);
        SpecialistsResponse? result = await response.Content.ReadFromJsonAsync<SpecialistsResponse>();

        response.IsSuccessStatusCode.ShouldBeTrue();
        result?.City.ShouldBe("London");
    }


    [Fact]
    public async Task PutSpecialistEndpoint_ShouldReturnNotFound()
    {
        var command = new PutSpecialistCommand(
            Guid.NewGuid(),
            SeedData.TestUserId,
            "123456789",
            "Test Description",
            "London"
        );

        var request = new HttpRequestMessage(HttpMethod.Put, $"specialists/{command.Id}")
        {
            Content = JsonContent.Create(command)
        };

        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.SendAsync(request);

        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PutSpecialistEndpoint_ShouldReturnForbidden()
    {
        var command = new PutSpecialistCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "123456789",
            "Test Description",
            "London"
        );

        var request = new HttpRequestMessage(HttpMethod.Put, $"specialists/{Guid.NewGuid()}")
        {
            Content = JsonContent.Create(command)
        };

        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.SendAsync(request);

        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteSpecialistEndpoint_ShouldReturnOk()
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"specialists/{SeedData.TestSpecialistToDeleteId}");
        string token = await GenerateAdminToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.SendAsync(request);
        response.IsSuccessStatusCode.ShouldBeTrue();
    }

    [Fact]
    public async Task DeleteSpecialistEndpoint_WrongRole_ShouldReturnForbidden()
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"specialists/{SeedData.TestSpecialistToDeleteId}");
        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.SendAsync(request);
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteSpecialistEndpoint_ShouldReturnNotFound()
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"specialists/{Guid.NewGuid()}");
        string token = await GenerateAdminToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.SendAsync(request);
        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetBySpecializationSpecialistsEndpoint_ShouldReturnSpecialists()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"specialists/specialization/{SeedData.TestSpecializationId}");
        HttpResponseMessage response = await _client.SendAsync(request);
        List<SpecialistsResponse>? result = await response.Content.ReadFromJsonAsync<List<SpecialistsResponse>>();

        response.IsSuccessStatusCode.ShouldBeTrue();
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetBySpecializationSpecialistsEndpoint_ShouldReturnNotFound()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"specialists/specialization/{Guid.NewGuid()}");
        HttpResponseMessage response = await _client.SendAsync(request);

        response.IsSuccessStatusCode.ShouldBeFalse();
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

}
