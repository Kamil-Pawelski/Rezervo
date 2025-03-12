using Application.Bookings;
using Application.Bookings.Create;
using Application.Bookings.Delete;
using Application.Bookings.Get;
using Application.Bookings.GetById;
using Domain.Bookings;
using Domain.Common;
using Domain.Schedules;
using Domain.Slots;
using Domain.Specialists;
using Domain.Specializations;
using Domain.Users;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace Tests.Bookings;

public sealed class BookingUnitTests : IDisposable
{
    private readonly ApplicationDbContext _context;

    public BookingUnitTests()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

        _context = new ApplicationDbContext(options);

        SeedData();
    }

    private Guid _specialistId;
    private Guid _userId;
    private Guid _secondUserId;
    private Guid _specializationId;
    private Guid _scheduleId;
    private Guid _slotId;
    private Guid _bookingId;
    private Guid _bookingSlotNotExistId;
    private Guid _bookingToDeleteId;

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

        _scheduleId = Guid.NewGuid();

        var schedule = new Schedule
        {
            Id = _scheduleId,
            SpecialistId = _specialistId,
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(16, 0),
            Date = DateOnly.FromDateTime(DateTime.Now)
        };

        _slotId = Guid.NewGuid();

        var slot = new Slot
        {
            Id = _slotId,
            ScheduleId = _scheduleId,
            StartTime = new TimeOnly(9, 0),
        };

         _bookingId = Guid.NewGuid();

        var booking = new Booking
        {
            Id = _bookingId,
            SlotId = _slotId,
            UserId = _userId,
        };

        _bookingSlotNotExistId = Guid.NewGuid();

        var bookingSlotNotExist = new Booking
        {
            Id = _bookingSlotNotExistId,
            SlotId = Guid.NewGuid(),
            UserId = _userId,
        };

        _bookingToDeleteId = Guid.NewGuid();

        var bookingToDelete = new Booking
        {
            Id = _bookingToDeleteId,
            SlotId = _slotId,
            UserId = _userId,
        };

        schedule.GenerateSlots(30);

        _context.Users.AddRange(user, secondUser);
        _context.Specializations.Add(specialization);
        _context.Specialists.AddRange(specialist);
        _context.Schedules.Add(schedule);
        _context.Slots.Add(slot);
        _context.Bookings.AddRange(booking, bookingSlotNotExist ,bookingToDelete);
        _context.SaveChanges();
    }

    [Fact]
    public async Task CreateBookling_ShouldReturnSuccess()
    {
        var command = new CreateBookingCommand
        (
           _slotId
        );

        var userContext = new TestUserContext
        {
            UserId = _userId
        };

        Result<string> result = await new CreateBookingCommandHandler(_context, userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe("Booking created.");
    }

    [Fact]
    public async Task CreateBooking_ShouldReturnError_NotFoundSlot()
    {
        var command = new CreateBookingCommand
        (
            Guid.NewGuid()
        );

        var userContextg = new TestUserContext
        {
            UserId = _userId
        };

        Result<string> result = await new CreateBookingCommandHandler(_context, userContextg).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("NotFoundSlot");
        result.Error.Description.ShouldBe("Slot with the given id does not exist");
    }

    [Fact]
    public async Task DeleteBooking_ShouldReturnSuccess()
    {
        var command = new DeleteBookingCommand
        (
            _bookingToDeleteId
        );

        var userContext = new TestUserContext
        {
            UserId = _userId
        };

        Result<string> result = await new DeleteBookingCommandHandler(_context, userContext).Handle(command, CancellationToken.None);
        
        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldBe("Booking deleted successfully.");
    }

    [Fact]
    public async Task DeleteBooking_ShouldReturnError_NotFoundBooking()
    {
        var command = new DeleteBookingCommand
        (
            Guid.NewGuid()
        );

        var userContext = new TestUserContext
        {
            UserId = _userId
        };

        Result<string> result = await new DeleteBookingCommandHandler(_context, userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("NotFoundBooking");
        result.Error.Description.ShouldBe("Booking with the given id does not exist");
    }

    [Fact]
    public async Task DeleteBooking_ShouldReturnError_Unauthorized()
    {
        var command = new DeleteBookingCommand
        (
            _bookingId
        );

        var userContext = new TestUserContext
        {
            UserId = _secondUserId
        };

        Result<string> result = await new DeleteBookingCommandHandler(_context, userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("Unauthorized");
        result.Error.Description.ShouldBe("You are not authorized to delete this booking");
    }

    [Fact]
    public async Task DeleteBooking_ShouldReturnError_NotFoundSlot()
    {
        var command = new DeleteBookingCommand
        (
            _bookingSlotNotExistId
        );

        var userContext = new TestUserContext
        {
            UserId = _userId
        };

        Result<string> result = await new DeleteBookingCommandHandler(_context, userContext).Handle(command, CancellationToken.None);
        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("NotFoundSlot");
        result.Error.Description.ShouldBe("Slot with the given id does not exist");
    }

    [Fact]
    public async Task GetBooking_ShouldReturnSuccess()
    {
        var query = new GetBookingQuery();

        var userContext = new TestUserContext
        {
            UserId = _userId
        };

        Result<List<BookingResponse>> result = await new GetBookingQueryHandler(_context, userContext).Handle(query, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
        result.Value.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetBooking_ShouldReturnError_NotFoundBookings()
    {
        var query = new GetBookingQuery();

        Result<List<BookingResponse>> result = await new GetBookingQueryHandler(_context, new TestUserContext { UserId = _secondUserId }).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("NotFoundBookings");
        result.Error.Description.ShouldBe("You don't have any bookings");
    }

    [Fact]
    public async Task GetByIdBooking_ShouldReturnSuccess()
    {
        var query = new GetByIdBookingQuery
        (
            _bookingId
        );

        Result<BookingResponse> result = await new GetByIdBookingQueryHandler(_context, new TestUserContext { UserId = _userId }).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetByIdBooking_ShouldReturnError_NotFoundBooking()
    {
        var query = new GetByIdBookingQuery
        (
            Guid.NewGuid()
        );

        Result<BookingResponse> result = await new GetByIdBookingQueryHandler(_context, new TestUserContext { UserId = _userId }).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe("NotFoundBooking");
        result.Error.Description.ShouldBe("Booking with the given id does not exist.");
    }

    public void Dispose() => _context.Dispose();
}
