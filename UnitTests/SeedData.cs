using Application.Abstractions.Authentication;
using Domain.Specialists;
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

    public static readonly Guid TestRoleId = Guid.NewGuid();
    public static readonly string TestRoleName = "Specialist";

    public static readonly Guid TestSpecializationId = Guid.NewGuid();
    public static readonly string TestSpecializationName = "Test Specialization";

    public static readonly Guid TestSpecialistId = Guid.NewGuid();
    public static readonly string TestSpecialistDescription = "Test Description";
    public static readonly string TestSpecialistPhoneNumber = "123456789";

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

        Role role = new()
        {
            Id = TestRoleId,
            Name = TestRoleName
        };

        dbContext.Users.Add(user);
        dbContext.Roles.Add(role);
        dbContext.SaveChanges();

        UserRole userRole = new()
        {
            UserId = TestUserId,
            RoleId = TestRoleId
        };

        dbContext.UserRoles.Add(userRole);
        dbContext.SaveChanges();
    }

    public static void SeedSpecialistTestData(ApplicationDbContext dbContext)
    {
        var specialization = new Specialization()
        {
            Id = TestSpecializationId,
            Name = TestSpecializationName
        };

        Guid userId = dbContext.Users.First().Id;

        var specialist = new Specialist()
        {
            Id = TestSpecialistId,
            UserId = userId,
            SpecializationId = TestSpecializationId,
            Description = TestSpecialistDescription,
            PhoneNumber = TestSpecialistPhoneNumber
        };

        dbContext.Specializations.Add(specialization);
        dbContext.Specialists.Add(specialist);
        dbContext.SaveChanges();
    }
}
