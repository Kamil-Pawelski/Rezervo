using Application.Schedules;
using Application.Schedules.Create;
using Application.Schedules.Delete;
using Application.Schedules.Get;
using Application.Schedules.GetById;
using Application.Schedules.Put;
using Domain.Common;
using Domain.Schedules;
using Domain.Specialists;
using Domain.Specializations;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Shouldly;


namespace Tests.Schedules;

public sealed class ScheduleUnitTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly TestUserContext _userContext;

    private Guid _specialistId;
    private Guid _userId;
    private Guid _secondSpecialistId;
    private Guid _secondUserId;
    private Guid _specializationId;
    private Guid _scheduleId;
    private Guid _secondScheduleId;
    private Guid _scheduleIdToDelete;
    public ScheduleUnitTests()
    {
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

        _secondUserId = Guid.NewGuid();

        var secondUser = new User
        {
            Id = _secondUserId,
            Email = "TestMail2@test.com",
            FirstName = "Test",
            LastName = "User",
            Username = "TestUser2",
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

        _secondSpecialistId = Guid.NewGuid();

        var secondSpecialist = new Specialist
        {
            Id = _secondSpecialistId,
            UserId = _secondUserId,
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
            Date = DateOnly.FromDateTime(DateTime.Now)
        };


        _secondScheduleId = Guid.NewGuid();

        var secondSchedule = new Schedule
        {
            Id = _secondScheduleId,
            SpecialistId = _specialistId,
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(16, 0),
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(2))
        };

        _scheduleIdToDelete = Guid.NewGuid();

        var scheduleToDelete = new Schedule
        {
            Id = _scheduleIdToDelete,
            SpecialistId = _specialistId,
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(16, 0),
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
        };

        schedule.GenerateSlots(30);

        _context.Users.AddRange(user, secondUser);
        _context.Specializations.Add(specialization);
        _context.Specialists.AddRange(specialist, secondSpecialist);
        _context.Schedules.AddRange(schedule, secondSchedule, scheduleToDelete);
        _context.SaveChanges();
    }


    [Fact]
    public async Task CreateSchedule_ShouldReturnSuccess()
    {
        var command = new CreateScheduleCommand(
            _specialistId,
            new TimeOnly(8, 0),
            new TimeOnly(16, 0),
            30,
            DateOnly.FromDateTime(DateTime.Now.AddDays(2))
        );

        Result<string> result = await new CreateScheduleCommandHandler(_context).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe($"Created schedule with 16 slots.");
    }


    [Fact]
    public async Task DeleteSchedule_ShouldReturnSuccess()
    {
        var command = new DeleteScheduleCommand(_scheduleIdToDelete);

        _userContext.UserId = _userId;
        Result<string> result = await new DeleteScheduleCommandHandler(_context, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe("Schedule deleted successfully.");
    }


    [Fact]
    public async Task DeleteSchedule_ShouldReturnError_NotFoundSchedule()
    {
        var command = new DeleteScheduleCommand(Guid.NewGuid());

        _userContext.UserId = _userId;
        Result<string> result = await new DeleteScheduleCommandHandler(_context, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("NotFoundSchedule");
        result.Error.Description.ShouldBe("Schedule with the given id does not exist.");
    }


    [Fact]
    public async Task DeleteSchedule_ShouldReturnError_Unauthorized()
    {
        var command = new DeleteScheduleCommand(_scheduleId);

        _userContext.UserId = Guid.NewGuid();
        Result<string> result = await new DeleteScheduleCommandHandler(_context, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("Unauthorized");
        result.Error.Description.ShouldBe("You are unauthorized to do this action.");
    }

    [Fact]
    public async Task GetSchedule_ShouldReturnSuccess()
    {
        var query = new GetScheduleQuery(_specialistId);

        Result<List<ScheduleDateResponse>> result = await new GetScheduleQueryHandler(_context).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetSchedule_ShouldReturnError_NotFoundSchedule()
    {
        var query = new GetScheduleQuery(_secondSpecialistId);

        Result<List<ScheduleDateResponse>> result = await new GetScheduleQueryHandler(_context).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("NoAvailableSlots");
        result.Error.Description.ShouldBe("The specialist has no available slots or does not have a schedule.");
    }

    [Fact]
    public async Task GetByIdScheduleSlots_ShouldReturnSuccess()
    {
        var query = new GetByIdScheduleSlotsQuery(_scheduleId);
        Result<List<SlotResponse>> result = await new GetByIdScheduleSlotsQueryHandler(_context).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetByIdScheduleSlots_ShouldReturnError_NotFoundScheduleSlots()
    {
        var query = new GetByIdScheduleSlotsQuery(_secondScheduleId);
        Result<List<SlotResponse>> result = await new GetByIdScheduleSlotsQueryHandler(_context).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("NotFoundSlots");
        result.Error.Description.ShouldBe("There are not slots available on this day.");
    }

    [Fact]
    public async Task PutSchedule_ShouldReturnSuccess()
    {
        var command = new PutScheduleCommand(
            _scheduleId,
            new TimeOnly(8, 0),
            new TimeOnly(18, 0)
            );

        _userContext.UserId = _userId;
        Result<string> result = await new PutScheduleCommandHandler(_context, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe($"Schedule updated successfully. New work time {command.StartTime}-{command.EndTime}.");
    }

    [Fact]
    public async Task PutSchedule_ShouldReturnError_NotFoundSchedule()
    {
        var command = new PutScheduleCommand(
            Guid.NewGuid(),
            new TimeOnly(8, 0),
            new TimeOnly(18, 0)
        );

        _userContext.UserId = _userId;
        Result<string> result = await new PutScheduleCommandHandler(_context, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("NotFoundSchedule");
        result.Error.Description.ShouldBe("Schedule with the given id does not exist.");
    }

    [Fact]
    public async Task PutSchedule_ShouldReturnError_Unauthorized()
    {
        var command = new PutScheduleCommand(
            _scheduleId,
            new TimeOnly(8, 0),
            new TimeOnly(18, 0)
        );

        _userContext.UserId = Guid.NewGuid();
        Result<string> result = await new PutScheduleCommandHandler(_context, _userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("Unauthorized");
        result.Error.Description.ShouldBe("You are unauthorized to do this action.");
    }


    public void Dispose() => _context.Dispose();
}
