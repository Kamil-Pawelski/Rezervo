using Domain.Users;
using Infrastructure.Database;

namespace Tests.Data;

public static class SeedRole
{
    public static readonly Guid TestRoleId = new("7A4A1573-AA6E-4504-885E-BBB3A04872F5");
    public static readonly Guid TestAdminRoleId = new("DC6C3733-C8B7-41FA-BFA0-B77EB710F9C3");

    public static void Seed(ApplicationDbContext dbContext)
    {
        var roles = new List<Role>()
            {
                new() { Id = TestAdminRoleId, Name = RolesNames.Admin },
                new() { Id = TestRoleId, Name = RolesNames.Specialist },
                new() { Id = new Guid("dd514642-f330-4950-ab3d-a3b454de9fc9"), Name = RolesNames.Client }
            };

        dbContext.Roles.AddRange(roles);
        dbContext.SaveChanges();
    }
}
