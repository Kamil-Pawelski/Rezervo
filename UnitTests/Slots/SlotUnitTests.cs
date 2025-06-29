using Application.Slots.Create;
using Application.Slots.Delete;
using Application.Slots.Put;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;
using Infrastructure.Authentication;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Tests.Seeder;
using static Tests.Seeder.SeedData;

namespace Tests.Slots;

public sealed class SlotUnitTests
{
    private readonly ApplicationDbContext _context;
    private readonly TestUserContext _userContext;
    private readonly SlotRepository _slotRepository;
    private readonly ScheduleRepository _scheduleRepository;

    public SlotUnitTests()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _userContext = new TestUserContext();
        _slotRepository = new SlotRepository(_context);
        _scheduleRepository = new ScheduleRepository(_context);

        SeedData.Initialize(_context);
    }

    [Fact]
    public async Task CreateSlot_ShouldReturnSuccess()
    {
        var command = new CreateSlotCommand(
            SeedScheduleAndSlots.TestScheduleId,
            new TimeOnly(8, 30)
        );

        _userContext.UserId = SeedUser.TestUserId;

        Result<string> result = await new CreateSlotCommandHandler(_slotRepository, _scheduleRepository, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task CreateSlot_ShouldReturnError_SlotAlreadyExist()
    {
        var command = new CreateSlotCommand(
            SeedScheduleAndSlots.TestScheduleId,
            new TimeOnly(9, 0)
        );

        _userContext.UserId = SeedUser.TestUserId;

        Result<string> result = await new CreateSlotCommandHandler(_slotRepository, _scheduleRepository, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(SlotErrors.SlotAlreadyExist(new TimeOnly(9, 0)).Code);
    }

    [Fact]
    public async Task CreateSlot_ShouldReturnError_NotFoundSchedule()
    {
        var command = new CreateSlotCommand(
            Guid.NewGuid(),
            new TimeOnly(10, 0)
        );

        _userContext.UserId = SeedUser.TestUserId;

        Result<string> result = await new CreateSlotCommandHandler(_slotRepository, _scheduleRepository, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(ScheduleErrors.NotFoundSchedule.Code);
    }

    [Fact]
    public async Task CreateSlot_ShouldReturnError_Unauthorized()
    {
        var command = new CreateSlotCommand(
            SeedScheduleAndSlots.TestScheduleId,
            new TimeOnly(10, 0)
        );

        _userContext.UserId = Guid.NewGuid();

        Result<string> result = await new CreateSlotCommandHandler(_slotRepository, _scheduleRepository, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(CommonErrors.Unauthorized.Code);
    }

    [Fact]
    public async Task DeleteSlot_ShouldReturnSuccess()
    {
        var command = new DeleteSlotCommand(SeedScheduleAndSlots.TestSlotToDeleteId);

        _userContext.UserId = SeedUser.TestUserId;

        var handler = new DeleteSlotCommandHandler(_slotRepository, _userContext);
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task DeleteSlot_ShouldReturnError_NotFoundSlot()
    {
        var command = new DeleteSlotCommand(Guid.NewGuid());

        _userContext.UserId = SeedUser.TestUserId;

        var handler = new DeleteSlotCommandHandler(_slotRepository, _userContext);
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(SlotErrors.NotFoundSlot.Code);
    }

    [Fact]
    public async Task DeleteSlot_ShouldReturnError_Unauthorized()
    {
        var command = new DeleteSlotCommand(SeedScheduleAndSlots.TestSlotId);

        _userContext.UserId = Guid.NewGuid();

        var handler = new DeleteSlotCommandHandler(_slotRepository, _userContext);
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(CommonErrors.Unauthorized.Code);
    }

    [Fact]
    public async Task PutSlot_ShouldReturnSuccess()
    {
        var command = new PutSlotCommand(
            SeedScheduleAndSlots.TestSlotId,
            new TimeOnly(12, 0)
        );

        _userContext.UserId = SeedUser.TestUserId;

        var handler = new PutSlotCommandHandler(_slotRepository, _userContext);
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        Slot? slot = await _context.Slots.FindAsync(SeedScheduleAndSlots.TestSlotId);
        slot.ShouldNotBeNull();
        slot.StartTime.ShouldBe(new TimeOnly(12, 0));
    }

    [Fact]
    public async Task PutSlot_ShouldReturnError_NotFoundSlot()
    {
        var command = new PutSlotCommand(
            Guid.NewGuid(),
            new TimeOnly(12, 0)
        );

        _userContext.UserId = SeedUser.TestUserId;

        var handler = new PutSlotCommandHandler(_slotRepository, _userContext);
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(SlotErrors.NotFoundSlot.Code);
    }

    [Fact]
    public async Task PutSlot_ShouldReturnError_Unauthorized()
    {
        var command = new PutSlotCommand(
            SeedScheduleAndSlots.TestSlotId,
            new TimeOnly(12, 0)
        );

        _userContext.UserId = Guid.NewGuid();

        var handler = new PutSlotCommandHandler(_slotRepository, _userContext);
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(CommonErrors.Unauthorized.Code);
    }

    [Fact]
    public async Task PutSlot_ShouldReturnError_InvalidTimeRange()
    {
        var command = new PutSlotCommand(
            SeedScheduleAndSlots.TestSlotId,
            new TimeOnly(7, 0)
        );

        _userContext.UserId = SeedUser.TestUserId;

        var handler = new PutSlotCommandHandler(_slotRepository, _userContext);
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(SlotErrors.InvalidTimeRange.Code);
    }
}
