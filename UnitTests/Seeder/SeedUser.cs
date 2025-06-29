using Application.Abstractions.Authentication;
using Domain.Users;
using Infrastructure.Database;

namespace Tests;

public static class SeedUser
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

    public static void Seed(ApplicationDbContext dbContext, IPasswordHasher passwordHasher)
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
}
