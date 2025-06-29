using Application.Abstractions.Authentication;
using Infrastructure.Authentication;
using Infrastructure.Database;
using Tests.Data;

namespace Tests.Seeder;

public static class SeedData
{
    private readonly static IPasswordHasher _passwordHasher = new PasswordHasher();
    public static async Task Initialize(ApplicationDbContext dbContext)
    {
        SeedRole.Seed(dbContext);
        SeedUser.Seed(dbContext, _passwordHasher);
        SeedSpecialist.Seed(dbContext);
        SeedScheduleAndSlots.Seed(dbContext);
        SeedBooking.Seed(dbContext);
        await dbContext.SaveChangesAsync();
    }
}
