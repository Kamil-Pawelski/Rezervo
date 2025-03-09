using Application.Slots.Create;
using Application.Slots.Delete;
using Application.Slots.Put;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;
using Domain.Specialists;
using Domain.Specializations;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shouldly;
using Tests.Users;
using static Tests.Specialists.SpecialistsUnitTests;

namespace Tests.Slots;

public sealed class SlotUnitTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly TestUserContext _userContext;

    private Guid _specialistId;
    private Guid _userId;
    private Guid _specializationId;
    private Guid _scheduleId;
    private Guid _slotId;
    private Guid _slotIdToDelete;

    public SlotUnitTests()
    {
        new ConfigurationBuilder()
            .AddUserSecrets<UserUnitTests>()
            .Build();

        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);
        _userContext = new TestUserContext();

        SeedData();

    }

    private void SeedData()
    {
        _userId = Guid.NewGuid();

        var user = new User
        {
            Id = _userId,
            Email = "TestMail@test.com",
            FirstName = "Test",
            LastName = "User",
            Username = "TestUser",
            PasswordHash = "Password123!"
        };

        _specializationId = Guid.NewGuid();

        var specialization = new Specialization
        {
            Id = _specializationId,
            Name = "Test Specialization",
        };


        _specialistId = Guid.NewGuid();

        var specialist = new Specialist
        {
            Id = _specialistId,
            UserId = _userId,
            SpecializationId = _specializationId,
            Description = "Test Description",
            PhoneNumber = "123456789",
            City = "Test"
        };


        _scheduleId = Guid.NewGuid();

        var schedule = new Schedule
        {
            Id = _scheduleId,
            SpecialistId = _specialistId,
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(16, 0),
            Date = new DateOnly(2025, 3, 10)
        };

        _slotId = Guid.NewGuid();

        var slot = new Slot
        {
            Id = _slotId,
            ScheduleId = _scheduleId,
            StartTime = new TimeOnly(9, 0),
        };

        _slotIdToDelete = Guid.NewGuid();

        var slotToDelete = new Slot
        {
            Id = _slotIdToDelete,
            ScheduleId = _scheduleId,
            StartTime = new TimeOnly(12, 0),
        };

        _context.Users.Add(user);
        _context.Specializations.Add(specialization);
        _context.Specialists.Add(specialist);
        _context.Schedules.Add(schedule);
        _context.Slots.Add(slot);
        _context.Slots.Add(slotToDelete);

        _context.SaveChanges();
    }

    [Fact]
    public async Task CreateSlot_ShouldReturnSuccess()
    {
        var command = new CreateSlotCommand(
            _scheduleId,
            new TimeOnly(8, 30)
        );

        _userContext.UserId = _userId;

        Result<string> result = await new CreateSlotCommandHandler(_context, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe("Slot created successfully");
    }

    [Fact]
    public async Task CreateSlot_ShouldReturnError_SlotAlreadyExist()
    {
        var command = new CreateSlotCommand(
            _scheduleId,
            new TimeOnly(9, 0)
        );

        _userContext.UserId = _userId;

        Result<string> result = await new CreateSlotCommandHandler(_context, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("SlotAlreadyExist");
        result.Error.Description.ShouldBe($"A slot already exists for the time {command.StartTime}. Please choose a different time.");
    }

    [Fact]
    public async Task CreateSlot_ShouldReturnError_NotFoundSchedule()
    {
        var command = new CreateSlotCommand(
            Guid.NewGuid(),
            new TimeOnly(10, 0)
        );

        _userContext.UserId = _userId;

        Result<string> result = await new CreateSlotCommandHandler(_context, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("NotFoundSchedule");
        result.Error.Description.ShouldBe("Schedule with the given id does not exist");
    }

    [Fact]
    public async Task CreateSlot_ShouldReturnError_Unauthorized()
    {
        var command = new CreateSlotCommand(
            _scheduleId,
            new TimeOnly(10, 0)
        );

        _userContext.UserId = Guid.NewGuid();

        Result<string> result = await new CreateSlotCommandHandler(_context, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("Unauthorized");
        result.Error.Description.ShouldBe("You are not authorized to create slots for this schedule");
    }

    [Fact]
    public async Task CreateSlot_ShouldReturnError_InvalidTimeRange_TooEarly()
    {
        var command = new CreateSlotCommand(
            _scheduleId,
            new TimeOnly(7, 30)
        );

        _userContext.UserId = _userId;

        Result<string> result = await new CreateSlotCommandHandler(_context, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("InvalidTimeRange");
        result.Error.Description.ShouldBe("EndTime must be later than StartTime.");
    }

    [Fact]
    public async Task CreateSlot_ShouldReturnError_InvalidTimeRange_TooLate()
    {
        var command = new CreateSlotCommand(
            _scheduleId,
            new TimeOnly(16, 30)
        );

        _userContext.UserId = _userId;

        Result<string> result = await new CreateSlotCommandHandler(_context, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("InvalidTimeRange");
        result.Error.Description.ShouldBe("EndTime must be later than StartTime.");
    }

    [Fact]
    public async Task DeleteSlot_ShouldReturnSuccess()
    {
        var command = new DeleteSlotCommand(_slotIdToDelete);

        _userContext.UserId = _userId;

        var handler = new DeleteSlotCommandHandler(_context, _userContext);
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe("Slot deleted successfully.");

        Slot? slot = await _context.Slots.FindAsync(_slotIdToDelete);
        slot.ShouldBeNull();
    }

    [Fact]
    public async Task DeleteSlot_ShouldReturnError_NotFoundSlot()
    {
        var command = new DeleteSlotCommand(Guid.NewGuid());

        _userContext.UserId = _userId;

        var handler = new DeleteSlotCommandHandler(_context, _userContext);
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("NotFoundSlot");
        result.Error.Description.ShouldBe("Slot with the given id does not exist");
    }

    [Fact]
    public async Task DeleteSlot_ShouldReturnError_Unauthorized()
    {
        var command = new DeleteSlotCommand(_slotId);

        _userContext.UserId = Guid.NewGuid();

        var handler = new DeleteSlotCommandHandler(_context, _userContext);
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("Unauthorized");
        result.Error.Description.ShouldBe("You are not allowed to delete this slot");
    }

    [Fact]
    public async Task PutSlot_ShouldReturnSuccess()
    {
        var command = new PutSlotCommand(
            _slotId, 
            new TimeOnly(12, 0)
            );

        _userContext.UserId = _userId;

        var handler = new PutSlotCommandHandler(_context, _userContext);
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe("Slot updated successfully.");

        Slot? slot = await _context.Slots.FindAsync(_slotId);
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

        _userContext.UserId = _userId;

        var handler = new PutSlotCommandHandler(_context, _userContext);
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("NotFoundSlot");
        result.Error.Description.ShouldBe("Slot with the given id does not exist");
    }

    [Fact]
    public async Task PutSlot_ShouldReturnError_Unauthorized()
    {
        var command = new PutSlotCommand(
            _slotId, 
            new TimeOnly(12, 0)
            );

        _userContext.UserId = Guid.NewGuid();

        var handler = new PutSlotCommandHandler(_context, _userContext);
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("Unauthorized");
        result.Error.Description.ShouldBe("You are not allowed to delete this slot");
    }

    [Fact]
    public async Task PutSlot_ShouldReturnError_InvalidTimeRange()
    {
        var command = new PutSlotCommand(
            _slotId, 
            new TimeOnly(7, 0)
            );

        _userContext.UserId = _userId;

        var handler = new PutSlotCommandHandler(_context, _userContext);
        Result<string> result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("InvalidTimeRange");
        result.Error.Description.ShouldBe("Slot time must be within the schedule time range");
    }


    public void Dispose() => _context.Dispose();

}
