using Application.Bookings.Get;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Bookings;

public sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
    app.MapGet("bookings/", [Authorize] async (ISender sender, CancellationToken cancellationToken) =>
    {
        var query = new GetBookingQuery();

        Result<List<BookingResponse>> result = await sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();

    });
}
