﻿using Application.Abstractions.Authentication;
using Domain.Specialists;
using Domain.Users;
using Infrastructure.Database;

public static class SeedData
{
    public static readonly Guid TestUserId = Guid.NewGuid();
    public static readonly string TestUserEmail = "EndpointTest@example.com";
    public static readonly string TestUsername = "EndpointTest";
    public static readonly string TestFirstName = "Endpoint";
    public static readonly string TestLastName = "Test";
    public static readonly string TestPassword = "Password123!";

    public static readonly Guid TestAdminId = Guid.NewGuid();
    public static readonly string TestAdminEmail = "AdminTest@example.com";
    public static readonly string TestAdminUsername = "AdminTest";
    public static readonly string TestAdminFirstName = "Admin";
    public static readonly string TestAdminLastName = "User";
    public static readonly string TestAdminPassword = "Admin123!";

    public static readonly Guid TestRoleId = Guid.NewGuid();
    public static readonly string TestRoleName = "Specialist";

    public static readonly Guid TestAdminRoleId = Guid.NewGuid();
    public static readonly string TestAdminRoleName = "Admin";

    public static readonly Guid TestSpecializationId = Guid.NewGuid();
    public static readonly string TestSpecializationName = "Test Specialization";

    public static readonly Guid TestSpecialistId = Guid.NewGuid();
    public static readonly string TestSpecialistDescription = "Test Description";
    public static readonly string TestSpecialistPhoneNumber = "123456789";
    public static readonly string TestSpecialistCity = "Warsaw";

    public static readonly Guid TestSpecialistToDeleteId = Guid.NewGuid();
    public static readonly string TestSpecialistToDeleteDescription = "Delete Specialist Description";
    public static readonly string TestSpecialistToDeletePhoneNumber = "987654321";
    public static readonly string TestSpecialistToDeleteCity = "Krakow";

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

        User admin = new()
        {
            Id = TestAdminId,
            Email = TestAdminEmail,
            Username = TestAdminUsername,
            FirstName = TestAdminFirstName,
            LastName = TestAdminLastName,
            PasswordHash = passwordHasher.Hash(TestAdminPassword)
        };

        Role specialistRole = new()
        {
            Id = TestRoleId,
            Name = TestRoleName
        };

        Role adminRole = new()
        {
            Id = TestAdminRoleId,
            Name = TestAdminRoleName
        };

        dbContext.Users.AddRange(user, admin);
        dbContext.Roles.AddRange(specialistRole, adminRole);
        dbContext.SaveChanges();

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
            UserId = userId,
            SpecializationId = TestSpecializationId,
            Description = TestSpecialistToDeleteDescription,
            PhoneNumber = TestSpecialistToDeletePhoneNumber,
            City = TestSpecialistToDeleteCity
        };

        dbContext.Specializations.Add(specialization);
        dbContext.Specialists.AddRange(specialist, specialistToDelete);
        dbContext.SaveChanges();
    }
}
