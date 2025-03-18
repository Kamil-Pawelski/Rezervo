using Domain.Common;

namespace Domain.Specialists;

public static class SpecialistErrors
{
    public static readonly Error NotFoundSpecialist = Error.NotFound("NotFoundSpecialist", "Specialist with the specified ID does not exist.");
}
