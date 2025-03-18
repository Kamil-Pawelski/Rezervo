﻿using Application.Schedules;
using Application.Schedules.Create;
using Application.Schedules.Delete;
using Application.Schedules.Get;
using Application.Schedules.GetById;
using Application.Schedules.Put;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;
using Infrastructure.Authentication;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using static Tests.SeedData;

namespace Tests.Schedules;

public sealed class ScheduleUnitTests
{
    private readonly ApplicationDbContext _context;

    public ScheduleUnitTests()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);

        SeedRoleData(_context);
        SeedUserTestData(_context, new PasswordHasher());
        SeedSpecialistTestData(_context);
        SeedScheduleAndSlotsTestData(_context);
        SeedBookingData(_context);
    }
    

    [Fact]
    public async Task CreateSchedule_ShouldReturnSuccess()
    {
        var command = new CreateScheduleCommand(
            TestSpecialistId,
            new TimeOnly(8, 0),
            new TimeOnly(16, 0),
            30,
            DateOnly.FromDateTime(DateTime.Now.AddDays(2))
        );

        Result<string> result = await new CreateScheduleCommandHandler(_context).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }


    [Fact]
    public async Task DeleteSchedule_ShouldReturnSuccess()
    {
        var command = new DeleteScheduleCommand(TestScheduleToDeleteId);

        Result<string> result = await new DeleteScheduleCommandHandler(_context, new TestUserContext { UserId = TestUserId }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }


    [Fact]
    public async Task DeleteSchedule_ShouldReturnError_NotFoundSchedule()
    {
        var command = new DeleteScheduleCommand(Guid.NewGuid());

        Result<string> result = await new DeleteScheduleCommandHandler(_context, new TestUserContext { UserId = TestUserId }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(ScheduleErrors.NotFoundSchedule.Code);
    }


    [Fact]
    public async Task DeleteSchedule_ShouldReturnError_Unauthorized()
    {
        var command = new DeleteScheduleCommand(TestScheduleToDeleteId);

        Result<string> result = await new DeleteScheduleCommandHandler(_context, new TestUserContext { UserId = Guid.NewGuid() }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(CommonErrors.Unauthorized.Code);

    }

    [Fact]
    public async Task GetSchedule_ShouldReturnSuccess()
    {
        var query = new GetScheduleQuery(TestSpecialistId);

        Result<List<ScheduleDateResponse>> result = await new GetScheduleQueryHandler(_context).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetSchedule_ShouldReturnError_NotFoundSchedule()
    {
        var query = new GetScheduleQuery(TestSpecialistToDeleteId);

        Result<List<ScheduleDateResponse>> result = await new GetScheduleQueryHandler(_context).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(ScheduleErrors.NoAvailableSlots.Code);
    }

    [Fact]
    public async Task GetByIdScheduleSlots_ShouldReturnSuccess()
    {
        var query = new GetByIdScheduleSlotsQuery(TestScheduleId);
        Result<List<SlotResponse>> result = await new GetByIdScheduleSlotsQueryHandler(_context).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetByIdScheduleSlots_ShouldReturnError_NotFoundScheduleSlots()
    {
        var query = new GetByIdScheduleSlotsQuery(TestScheduleToDeleteId);
        Result<List<SlotResponse>> result = await new GetByIdScheduleSlotsQueryHandler(_context).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(SlotErrors.NotFoundSlots.Code);
    }

    [Fact]
    public async Task PutSchedule_ShouldReturnSuccess()
    {
        var command = new PutScheduleCommand(
            TestScheduleId,
            new TimeOnly(8, 0),
            new TimeOnly(18, 0)
            );

        Result<string> result = await new PutScheduleCommandHandler(_context, new TestUserContext { UserId = TestUserId }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task PutSchedule_ShouldReturnError_NotFoundSchedule()
    {
        var command = new PutScheduleCommand(
            Guid.NewGuid(),
            new TimeOnly(8, 0),
            new TimeOnly(18, 0)
        );

        Result<string> result = await new PutScheduleCommandHandler(_context, new TestUserContext { UserId = TestUserId }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(ScheduleErrors.NotFoundSchedule.Code);
    }

    [Fact]
    public async Task PutSchedule_ShouldReturnError_Unauthorized()
    {
        var command = new PutScheduleCommand(
            TestScheduleId,
            new TimeOnly(8, 0),
            new TimeOnly(18, 0)
        );

        Result<string> result = await new PutScheduleCommandHandler(_context, new TestUserContext { UserId = Guid.NewGuid() }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(CommonErrors.Unauthorized.Code);
    }
}
