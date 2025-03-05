namespace Application.Bookings;

public sealed record BookingResponse(Guid Id, DateTime Date, string SpecialistFullName, string SpecializationName);

