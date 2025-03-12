using Domain.Common;

namespace Domain.Specialists;

public static class SpecialistErrors
{
    public static readonly Error NotFoundSpecialist = Error.NotFound("NotFoundSpecialist", "Specialist with the given id does not exist");
}
