using Application.Abstractions.Repositories;
using Application.Bookings;
using Application.Bookings.Create;
using Application.Bookings.Delete;
using Application.Bookings.Get;
using Application.Bookings.GetById;
using Domain.Bookings;
using Domain.Common;
using Domain.Slots;
using Infrastructure.Authentication;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Tests.Seeder;
using static Tests.Seeder.SeedData;
namespace Tests.Bookings;

public sealed class BookingUnitTests
{
    private readonly ApplicationDbContext _context;
    private readonly BookingRepository _bookingRepository;
    private readonly SlotRepository _slotRepository;

    public BookingUnitTests()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _slotRepository = new SlotRepository(_context);
        _bookingRepository = new BookingRepository(_context);

        SeedData.Initialize(_context);
    }

    [Fact]
    public async Task CreateBooking_ShouldReturnSuccess()
    {
        var command = new CreateBookingCommand(SeedScheduleAndSlots.TestSlotForBooking2Id);

        Result<string> result = await new CreateBookingCommandHandler(_slotRepository, _bookingRepository, new TestUserContext { UserId = SeedUser.TestUserId }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task CreateBooking_ShouldReturnError_NotFoundSlot()
    {
        var command = new CreateBookingCommand(Guid.NewGuid());

        Result<string> result = await new CreateBookingCommandHandler(_slotRepository, _bookingRepository, new TestUserContext { UserId = SeedUser.TestUserId }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(SlotErrors.NotFoundSlot.Code);
    }

    [Fact]
    public async Task DeleteBooking_ShouldReturnSuccess()
    {
        var command = new DeleteBookingCommand(SeedBooking.TestBookingToDeleteId);

        var userContext = new TestUserContext { UserId = SeedUser.TestUserId };

        Result<string> result = await new DeleteBookingCommandHandler(_slotRepository, _bookingRepository, userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task DeleteBooking_ShouldReturnError_NotFoundBooking()
    {
        var command = new DeleteBookingCommand(Guid.NewGuid());

        Result<string> result = await new DeleteBookingCommandHandler(_slotRepository, _bookingRepository, new TestUserContext { UserId = SeedUser.TestUserId }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(BookingErrors.NotFoundBooking.Code);
    }

    [Fact]
    public async Task DeleteBooking_ShouldReturnError_Unauthorized()
    {
        var command = new DeleteBookingCommand(SeedBooking.TestBookingId);
 

        Result<string> result = await new DeleteBookingCommandHandler(_slotRepository, _bookingRepository, new TestUserContext { UserId = SeedUser.TestUserId2 }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(CommonErrors.Unauthorized.Code);
    }

    [Fact]
    public async Task GetBooking_ShouldReturnSuccess()
    {
        var query = new GetBookingQuery();

        Result<List<BookingResponse>> result = await new GetBookingQueryHandler(_bookingRepository, new TestUserContext { UserId = SeedUser.TestUserId }).Handle(query, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
        result.Value.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetBooking_ShouldReturnError_NotFoundBookings()
    {
        var query = new GetBookingQuery();

        Result<List<BookingResponse>> result = await new GetBookingQueryHandler(_bookingRepository, new TestUserContext { UserId = SeedUser.TestUserId2 }).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(BookingErrors.NotFoundBookings.Code);
    }

    [Fact]
    public async Task GetByIdBooking_ShouldReturnSuccess()
    {
        var query = new GetByIdBookingQuery(SeedBooking.TestBookingId);

        Result<BookingResponse> result = await new GetByIdBookingQueryHandler(_bookingRepository, new TestUserContext { UserId = SeedUser.TestUserId }).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetByIdBooking_ShouldReturnError_NotFoundBooking()
    {
        var query = new GetByIdBookingQuery(Guid.NewGuid());

        Result<BookingResponse> result = await new GetByIdBookingQueryHandler(_bookingRepository, new TestUserContext { UserId = SeedUser.TestUserId }).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(BookingErrors.NotFoundBooking.Code);
    }
}
