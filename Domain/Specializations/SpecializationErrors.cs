using Domain.Common;

namespace Domain.Specializations;

public static class SpecializationErrors
{
    public static readonly Error NotFoundSpecialziation = Error.NotFound("NotFoundSpecialization", "Specialization with the given name does not exist.");
    public static readonly Error NotFoundSpecializations = Error.NotFound("NotFoundSpecializations", "No specializations found.");
}
