using Application.Bookings.Delete;
using Domain.Common;
using MediatR;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Bookings;

public sealed class Delete : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) => 
    app.MapDelete("bookings/{id}", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
    {
        Result<string> result = await sender.Send(new DeleteBookingCommand(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
    });
}
