using Application.Abstractions.Authentication;
using Domain.Bookings;
using Domain.Schedules;
using Domain.Slots;
using Domain.Specialists;
using Domain.Specializations;
using Domain.Users;
using Infrastructure.Database;

namespace Tests;

public static class SeedData
{
    public static readonly Guid TestUserId = Guid.NewGuid();
    public static readonly string TestUserEmail = "EndpointTest@example.com";
    public static readonly string TestUsername = "EndpointTest";
    public static readonly string TestFirstName = "Endpoint";
    public static readonly string TestLastName = "Test";
    public static readonly string TestPassword = "Password123!";

    public static readonly Guid TestUserId2 = Guid.NewGuid();
    public static readonly string TestUserEmail2 = "EndpointTest2@example.com";
    public static readonly string TestUsername2 = "EndpointTest2";

    public static readonly Guid TestAdminId = Guid.NewGuid();
    public static readonly string TestAdminEmail = "AdminTest@example.com";
    public static readonly string TestAdminUsername = "AdminTest";


    public static readonly Guid TestRoleId = new("7A4A1573-AA6E-4504-885E-BBB3A04872F5");

    public static readonly Guid TestAdminRoleId = new("DC6C3733-C8B7-41FA-BFA0-B77EB710F9C3");

    public static readonly Guid TestSpecializationId = Guid.NewGuid();
    public static readonly string TestSpecializationName = "Test Specialization";

    public static readonly Guid TestSpecialistId = Guid.NewGuid();
    public static readonly string TestSpecialistDescription = "Test Description";
    public static readonly string TestSpecialistPhoneNumber = "123456789";
    public static readonly string TestSpecialistCity = "Warsaw";

    public static readonly Guid TestSpecialistToDeleteId = Guid.NewGuid();

    public static readonly Guid TestScheduleId = Guid.NewGuid();
    public static readonly Guid TestScheduleToDeleteId = Guid.NewGuid();

    public static readonly Guid TestSlotId = Guid.NewGuid();
    public static readonly TimeOnly TestSlotStartTime = new(9, 0, 0);

    public static readonly Guid TestSlotToDeleteId = Guid.NewGuid();
    public static readonly TimeOnly TestSlotToDeleteStartTime = new(12, 0, 0);

    public static readonly Guid TestBookingId = Guid.NewGuid();
    public static readonly Guid TestBookingToDeleteId = Guid.NewGuid();
    public static readonly Guid TestSlotForBookingId = Guid.NewGuid();
    public static readonly Guid TestSlotForBooking2Id = Guid.NewGuid();

    public static void SeedRoleData(ApplicationDbContext dbContext)
    {
        var roles = new List<Role>()
        {
              new() { Id = new Guid("dc6c3733-c8b7-41fa-bfa0-b77eb710f9c3"), Name = RolesNames.Admin },
              new() { Id = new Guid("7a4a1573-aa6e-4504-885e-bbb3a04872f5"), Name = RolesNames.Specialist },
              new() { Id = new Guid("dd514642-f330-4950-ab3d-a3b454de9fc9"), Name = RolesNames.Client }
        };

        dbContext.Roles.AddRange(roles);
        dbContext.SaveChanges();
    }

    public static void SeedUserTestData(ApplicationDbContext dbContext, IPasswordHasher passwordHasher)
    {
        User user = new()
        {
            Id = TestUserId,
            Email = TestUserEmail,
            Username = TestUsername,
            FirstName = TestFirstName,
            LastName = TestLastName,
            PasswordHash = passwordHasher.Hash(TestPassword)
        };

        User user2 = new()
        {
            Id = TestUserId2,
            Email = TestUserEmail2,
            Username = TestUsername2,
            FirstName = TestFirstName,
            LastName = TestLastName,
            PasswordHash = passwordHasher.Hash(TestPassword)
        };

        User admin = new()
        {
            Id = TestAdminId,
            Email = TestAdminEmail,
            Username = TestAdminUsername,
            FirstName = TestFirstName,
            LastName = TestLastName,
            PasswordHash = passwordHasher.Hash(TestPassword)
        };

        UserRole userRole = new()
        {
            UserId = TestUserId,
            RoleId = TestRoleId
        };

        UserRole adminUserRole = new()
        {
            UserId = TestAdminId,
            RoleId = TestAdminRoleId
        };

        dbContext.Users.AddRange(user, admin, user2);
        dbContext.UserRoles.AddRange(userRole, adminUserRole);
        dbContext.SaveChanges();
    }

    public static void SeedSpecialistTestData(ApplicationDbContext dbContext)
    {
        var specialization = new Specialization()
        {
            Id = TestSpecializationId,
            Name = TestSpecializationName
        };

        Guid userId = dbContext.Users.First(u => u.Id == TestUserId).Id;

        var specialist = new Specialist()
        {
            Id = TestSpecialistId,
            UserId = userId,
            SpecializationId = TestSpecializationId,
            Description = TestSpecialistDescription,
            PhoneNumber = TestSpecialistPhoneNumber,
            City = TestSpecialistCity
        };

        var specialistToDelete = new Specialist()
        {
            Id = TestSpecialistToDeleteId,
            UserId = TestAdminId,
            SpecializationId = TestSpecializationId,
            Description = TestSpecialistDescription,
            PhoneNumber = TestSpecialistPhoneNumber,
            City = TestSpecialistCity
        };

        dbContext.Specializations.Add(specialization);
        dbContext.Specialists.AddRange(specialist, specialistToDelete);
        dbContext.SaveChanges();
    }

    public static void SeedScheduleAndSlotsTestData(ApplicationDbContext dbContext)
    {
        var schedule = new Schedule()
        {
            Id = TestScheduleId,
            SpecialistId = TestSpecialistId,
            StartTime = new TimeOnly(8, 0, 0),
            EndTime = new TimeOnly(16, 0, 0),
            Date = DateOnly.FromDateTime(DateTime.Now)
        };

        var scheduleToDelete = new Schedule()
        {
            Id = TestScheduleToDeleteId,
            SpecialistId = TestSpecialistId,
            StartTime = new TimeOnly(8, 0),
            EndTime = new TimeOnly(16, 0),
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
        };

        var slot = new Slot
        {
            Id = TestSlotId,
            ScheduleId = TestScheduleId,
            StartTime = TestSlotStartTime,
            Status = Status.Available
        };

        var slotForBooking = new Slot
        {
            Id = TestSlotForBookingId,
            ScheduleId = TestScheduleId,
            StartTime = new TimeOnly(11, 20),
            Status = Status.Available
        };

        var slotForBooking2 = new Slot
        {
            Id = TestSlotForBooking2Id,
            ScheduleId = TestScheduleId,
            StartTime = new TimeOnly(11, 40),
            Status = Status.Available
        };

        var slotToDelete = new Slot
        {
            Id = TestSlotToDeleteId,
            ScheduleId = TestScheduleId,
            StartTime = TestSlotToDeleteStartTime,
            Status = Status.Available
        };

        dbContext.Schedules.AddRange(schedule, scheduleToDelete);
        dbContext.Slots.AddRange(slot, slotForBooking, slotForBooking2, slotToDelete);
        dbContext.SaveChanges();
    }

    public static void SeedBookingData(ApplicationDbContext dbContext)
    {
        var booking = new Booking
        {
            Id = TestBookingId,
            SlotId = TestSlotForBooking2Id,
            UserId = TestUserId,
        };

        var bookingToDelete = new Booking
        {
            Id = TestBookingToDeleteId,
            SlotId = TestSlotId,
            UserId = TestUserId,
        };

        dbContext.Bookings.AddRange(booking, bookingToDelete);
        dbContext.SaveChanges();
    }
}
