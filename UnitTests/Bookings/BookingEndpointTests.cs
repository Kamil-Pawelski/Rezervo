using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Application.Bookings.Create;
using Application.Users.Login;
using Shouldly;
using Tests.Response;
using Web.Api;

namespace Tests.Bookings;

[Collection("Factory")]
public sealed class BookingEndpointTests(CustomWebApplicationFactory<Program> factory)
{
    private readonly HttpClient _client = factory.CreateClient();


    private async Task<string> GenerateUserToken()
    {
        var command = new LoginUserCommand(SeedData.TestUsername, SeedData.TestPassword);
        HttpResponseMessage response = await _client.PostAsJsonAsync("users/login", command);
        string result = await response.Content.ReadAsStringAsync();
        TokenResponse? json = JsonSerializer.Deserialize<TokenResponse>(result);
        return json!.Token;
    }

    [Fact]
    public async Task CreateBooking_ShouldReturnSuccess()
    {
        var command = new CreateBookingCommand
        (
           SeedData.TestSlotForBookingId
        );

        var request = new HttpRequestMessage(HttpMethod.Post, "bookings")
        {
            Content = JsonContent.Create(command)
        };

        string token = await GenerateUserToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await _client.SendAsync(request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }

    [Fact]
    public async Task DeleteBooking_ShouldReturnSuccess()
    {        
        var request = new HttpRequestMessage(HttpMethod.Delete, $"bookings/{SeedData.TestBookingToDeleteId}");
        
        string token = await GenerateUserToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        HttpResponseMessage response = await _client.SendAsync(request);
        
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetBooking_ShouldReturnSuccess()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"bookings/");

        string token = await GenerateUserToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.SendAsync(request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetBookingById_ShouldReturnSuccess()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"bookings/{SeedData.TestBookingId}");

        string token = await GenerateUserToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.SendAsync(request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}

