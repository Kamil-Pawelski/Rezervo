﻿using Application.Users.Login;
using System.Net.Http.Json;
using Tests.Response;
using System.Text.Json;
using Application.Schedules.Create;
using Shouldly;
using System.Net;
using Web.Api;
using Application.Schedules;
using Application.Schedules.Get;
using Application.Schedules.Put;
using System.Net.Http.Headers;
using Tests.IntegrationTestsConfiguration;
using Tests.Seeder;

namespace Tests.Schedules;

[Collection("Factory")]
public sealed class ScheduleIntegrationTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
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
    public async Task CreateSchedule_ShouldReturnSuccess()
    {
        var command = new CreateScheduleCommand(
            SeedSpecialist.TestSpecialistId,
            new TimeOnly(8, 0),
            new TimeOnly(16, 0),
            30,
            DateOnly.FromDateTime(DateTime.Now.AddDays(2))
            );

        var request = new HttpRequestMessage(HttpMethod.Post, "schedules")
        {
            Content = JsonContent.Create(command)
        };

        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await Client.SendAsync(request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        string? result = await response.Content.ReadFromJsonAsync<string>();
        result.ShouldBe("Created schedule with 16 slots.");
    }

    [Fact]
    public async Task DeleteSchedule_SchouldReturnSuccess()
    {
        var request = new HttpRequestMessage(HttpMethod.Delete, $"schedules/{SeedScheduleAndSlots.TestScheduleToDeleteId}");
        string token = await GenerateSpecialistToken();

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await Client.SendAsync(request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        string? result = await response.Content.ReadFromJsonAsync<string>();
        result.ShouldBe("Schedule deleted successfully.");
    }

    [Fact]
    public async Task GetSchedule_ShouldReturnSuccess()
    {
        var query = new GetScheduleQuery(SeedSpecialist.TestSpecialistId);
        var request = new HttpRequestMessage(HttpMethod.Get, $"schedules/")
        {
            Content = JsonContent.Create(query)
        };

        HttpResponseMessage response = await Client.SendAsync(request);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        List<ScheduleResponse>? result = await response.Content.ReadFromJsonAsync<List<ScheduleResponse>>();
        result?.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetByIdScheduleSlots_ShouldReturnSuccess()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"schedules/{SeedScheduleAndSlots.TestScheduleId}");

        HttpResponseMessage response = await Client.SendAsync(request);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        List<SlotResponse>? result = await response.Content.ReadFromJsonAsync<List<SlotResponse>>();
        result?.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task PutSchedule_ShouldReturnSuccess()
    {
        var command = new PutScheduleCommand(
            SeedScheduleAndSlots.TestScheduleId,
            new TimeOnly(5, 0),
            new TimeOnly(16, 0)            
            );

        var request = new HttpRequestMessage(HttpMethod.Put, $"schedules/{SeedScheduleAndSlots.TestScheduleId}")
        {
            Content = JsonContent.Create(command)
        };
        
        string token = await GenerateSpecialistToken();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await Client.SendAsync(request);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        string? result = await response.Content.ReadFromJsonAsync<string>();
        result.ShouldBe($"Schedule updated successfully. New work time {command.StartTime}-{command.EndTime}.");
    }
}
