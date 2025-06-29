using Domain.Specialists;
using Domain.Specializations;
using Infrastructure.Database;

namespace Tests;

public static class SeedSpecialist
{
    public static readonly Guid TestSpecializationId = Guid.NewGuid();
    public static readonly string TestSpecializationName = "Test Specialization";

    public static readonly Guid TestSpecialistId = Guid.NewGuid();
    public static readonly string TestSpecialistDescription = "Test Description";
    public static readonly string TestSpecialistPhoneNumber = "123456789";
    public static readonly string TestSpecialistCity = "Warsaw";

    public static readonly Guid TestSpecialistToDeleteId = Guid.NewGuid();

    public static void Seed(ApplicationDbContext dbContext)
    {
        var specialization = new Specialization()
        {
            Id = TestSpecializationId,
            Name = TestSpecializationName
        };

        Guid userId = dbContext.Users.First(u => u.Id == SeedUser.TestUserId).Id;

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
            UserId = SeedUser.TestAdminId,
            SpecializationId = TestSpecializationId,
            Description = TestSpecialistDescription,
            PhoneNumber = TestSpecialistPhoneNumber,
            City = TestSpecialistCity
        };

        dbContext.Specializations.Add(specialization);
        dbContext.Specialists.AddRange(specialist, specialistToDelete);
        dbContext.SaveChanges();
    }
}
