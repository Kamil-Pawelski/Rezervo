using Domain.Bookings;

namespace Application.Abstractions.Repositories;

public interface IBookingRepository
{
    Task AddAsync(Booking booking, CancellationToken cancellationToken);
    Task DeleteAsync(Booking booking, CancellationToken cancellationToken);
    Task<List<Booking>> GetAllAsync(Guid userId, CancellationToken cancellationToken);
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
