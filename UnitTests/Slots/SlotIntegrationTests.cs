using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Slots.Create;
using Application.Slots.Put;
using Application.Users.Login;
using Shouldly;
using Tests.IntegrationTestsConfiguration;

namespace Tests.Slots;


[Collection("Factory")]
public sealed class SlotIntegrationTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
    private async Task<string> GenerateSpecialistToken()
    {
        var command = new LoginUserCommand(SeedUser.TestUsername, SeedUser.TestPassword);
        HttpResponseMessage response = await Client.PostAsJsonAsync("users/login", command);
        string result = await response.Content.ReadAsStringAsync();
        TokenResponse? json = JsonSerializer.Deserialize<TokenResponse>(result);
        return json!.Token;
    }

    [Fact]
    public async Task CreateSlot_ShouldReturnSuccess()
    {

        var command = new CreateSlotCommand(
            SeedScheduleAndSlots.TestScheduleId, 
            new TimeOnly(10, 0)
            );

        var request = new HttpRequestMessage(HttpMethod.Post, "slots")
        {
            Content = JsonContent.Create(command)
        };
        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await Client.SendAsync(request);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        string? result = await response.Content.ReadFromJsonAsync<string>();
        result.ShouldBe("Slot created successfully.");
    }

    [Fact]
    public async Task DeleteSlot_ShouldReturnSuccess()
    {

        var request = new HttpRequestMessage(HttpMethod.Delete, $"slots/{SeedScheduleAndSlots.TestSlotToDeleteId}");

        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await Client.SendAsync(request);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        string? result = await response.Content.ReadFromJsonAsync<string>();
        result.ShouldBe("Slot deleted successfully.");
    }
    

    [Fact]
    public async Task PutSlot_ShouldReturnSuccess()
    {
        var command = new PutSlotCommand(
            SeedScheduleAndSlots.TestSlotId,
            new TimeOnly(14, 0)
        );

        var request = new HttpRequestMessage(HttpMethod.Put, $"slots/{SeedScheduleAndSlots.TestSlotId}")
        {
            Content = JsonContent.Create(command)
        };
        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await Client.SendAsync(request);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        string? result = await response.Content.ReadFromJsonAsync<string>();
        result.ShouldBe("Slot updated successfully.");
    }    
    
    private class TokenResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; init; } = string.Empty;
    }
}
