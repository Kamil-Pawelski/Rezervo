﻿using Application.Bookings;
using Application.Bookings.Create;
using Application.Bookings.Delete;
using Application.Bookings.Get;
using Application.Bookings.GetById;
using Domain.Bookings;
using Domain.Common;
using Domain.Slots;
using Infrastructure.Authentication;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using static Tests.SeedData;
namespace Tests.Bookings;

public sealed class BookingUnitTests
{
    private readonly ApplicationDbContext _context;

    public BookingUnitTests()
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
    public async Task CreateBooking_ShouldReturnSuccess()
    {
        var command = new CreateBookingCommand(TestSlotForBooking2Id);

        Result<string> result = await new CreateBookingCommandHandler(_context, new TestUserContext { UserId = TestUserId }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task CreateBooking_ShouldReturnError_NotFoundSlot()
    {
        var command = new CreateBookingCommand(Guid.NewGuid());

        Result<string> result = await new CreateBookingCommandHandler(_context, new TestUserContext { UserId = TestUserId }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(SlotErrors.NotFoundSlot.Code);
    }

    [Fact]
    public async Task DeleteBooking_ShouldReturnSuccess()
    {
        var command = new DeleteBookingCommand(TestBookingToDeleteId);

        var userContext = new TestUserContext { UserId = TestUserId };

        Result<string> result = await new DeleteBookingCommandHandler(_context, userContext).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
    }

    [Fact]
    public async Task DeleteBooking_ShouldReturnError_NotFoundBooking()
    {
        var command = new DeleteBookingCommand(Guid.NewGuid());

        Result<string> result = await new DeleteBookingCommandHandler(_context, new TestUserContext { UserId = TestUserId }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(BookingErrors.NotFoundBooking.Code);
    }

    [Fact]
    public async Task DeleteBooking_ShouldReturnError_Unauthorized()
    {
        var command = new DeleteBookingCommand(TestBookingId);
 

        Result<string> result = await new DeleteBookingCommandHandler(_context, new TestUserContext { UserId = TestUserId2 }).Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(CommonErrors.Unauthorized.Code);
    }

    [Fact]
    public async Task GetBooking_ShouldReturnSuccess()
    {
        var query = new GetBookingQuery();

        Result<List<BookingResponse>> result = await new GetBookingQueryHandler(_context, new TestUserContext { UserId = TestUserId }).Handle(query, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue();
        result.Value.Count.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task GetBooking_ShouldReturnError_NotFoundBookings()
    {
        var query = new GetBookingQuery();

        Result<List<BookingResponse>> result = await new GetBookingQueryHandler(_context, new TestUserContext { UserId = TestUserId2 }).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(BookingErrors.NotFoundBookings.Code);
    }

    [Fact]
    public async Task GetByIdBooking_ShouldReturnSuccess()
    {
        var query = new GetByIdBookingQuery(TestBookingId);

        Result<BookingResponse> result = await new GetByIdBookingQueryHandler(_context, new TestUserContext { UserId = TestUserId }).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetByIdBooking_ShouldReturnError_NotFoundBooking()
    {
        var query = new GetByIdBookingQuery(Guid.NewGuid());

        Result<BookingResponse> result = await new GetByIdBookingQueryHandler(_context, new TestUserContext { UserId = TestUserId }).Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Error.Code.ShouldBe(BookingErrors.NotFoundBooking.Code);
    }
}
