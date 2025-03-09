using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Slots.Create;
using Application.Slots.Delete;
using Application.Slots.Put;
using Application.Users.Login;
using Domain.Slots;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Web.Api;

namespace Tests.Slots;


[Collection("Factory")]
public sealed class SlotEndpointsTests(CustomWebApplicationFactory<Program> factory)
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

    [Fact]
    public async Task CreateSlot_ShouldReturnSuccess()
    {

        var command = new CreateSlotCommand(
            SeedData.TestScheduleId, 
            new TimeOnly(10, 0)
            );

        var request = new HttpRequestMessage(HttpMethod.Post, "slots")
        {
            Content = JsonContent.Create(command)
        };
        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.SendAsync(request);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        string? result = await response.Content.ReadFromJsonAsync<string>();
        result.ShouldBe("Slot created successfully");
    }

    [Fact]
    public async Task CreateSlot_ShouldReturnError_SlotAlreadyExist()
    {

        var command = new CreateSlotCommand(
            SeedData.TestScheduleId,
            SeedData.TestSlotStartTime
        );

        var request = new HttpRequestMessage(HttpMethod.Post, "slots")
        {
            Content = JsonContent.Create(command)
        };
        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.SendAsync(request);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Conflict);

        ErrorResponse? result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result?.Description.ShouldBe($"A slot already exists for the time {command.StartTime}. Please choose a different time.");
        result?.Code.ShouldBe("SlotAlreadyExist");
    }

    [Fact]
    public async Task CreateSlot_ShouldReturnError_ScheduleNotFound()
    {

        var command = new CreateSlotCommand(
            Guid.NewGuid(), 
            SeedData.TestSlotStartTime
        );

        var request = new HttpRequestMessage(HttpMethod.Post, "slots")
        {
            Content = JsonContent.Create(command)
        };
        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.SendAsync(request);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);

        ErrorResponse? result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result?.Description.ShouldBe("Schedule with the given id does not exist");
        result?.Code.ShouldBe("NotFoundSchedule");
    }

    [Fact]
    public async Task DeleteSlot_ShouldReturnSuccess()
    {

        var request = new HttpRequestMessage(HttpMethod.Delete, $"slots/{SeedData.TestSlotToDeleteId}");
        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.SendAsync(request);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        string? result = await response.Content.ReadFromJsonAsync<string>();
        result.ShouldBe("Slot deleted successfully.");
    }

    [Fact]
    public async Task DeleteSlot_ShouldReturnError_NotFoundSlot()
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"slots/{Guid.NewGuid()}");
        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.SendAsync(request);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);

        ErrorResponse? result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result?.Description.ShouldBe("Slot with the given id does not exist");
        result?.Code.ShouldBe("NotFoundSlot");
    }

    [Fact]
    public async Task PutSlot_ShouldReturnSuccess()
    {
        var command = new PutSlotCommand(
            SeedData.TestSlotId,
            new TimeOnly(14, 0)
        );

        var request = new HttpRequestMessage(HttpMethod.Put, $"slots/{SeedData.TestSlotId}")
        {
            Content = JsonContent.Create(command)
        };
        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.SendAsync(request);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        string? result = await response.Content.ReadFromJsonAsync<string>();
        result.ShouldBe("Slot updated successfully.");
    }

    [Fact]
    public async Task PutSlot_ShouldReturnError_NotFoundSlot()
    {
        var command = new PutSlotCommand(
            Guid.NewGuid(),
            new TimeOnly(12, 0)
        );

        var request = new HttpRequestMessage(HttpMethod.Put, $"slots/{Guid.NewGuid()}")
        {
            Content = JsonContent.Create(command)
        };
        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.SendAsync(request);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);

        ErrorResponse? result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result?.Description.ShouldBe("Slot with the given id does not exist");
        result?.Code.ShouldBe("NotFoundSlot");
    }

    [Fact]
    public async Task PutSlot_ShouldReturnError_InvalidTimeRange()
    {
        var command = new PutSlotCommand(
            SeedData.TestSlotId,
            new TimeOnly(7, 0)
        );

        var request = new HttpRequestMessage(HttpMethod.Put, $"slots/{SeedData.TestSlotId}")
        {
            Content = JsonContent.Create(command)
        };
        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.SendAsync(request);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Conflict);

        ErrorResponse? result = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        result?.Description.ShouldBe("Slot time must be within the schedule time range");
        result?.Code.ShouldBe("InvalidTimeRange");
    }

    private class TokenResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; init; } = string.Empty;
    }
}
