using Application.Bookings;
using Application.Bookings.GetById;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Bookings;

public sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) => 
    app.MapGet("bookings/{id}", [Authorize] async ([FromRoute] Guid id, ISender sender, CancellationToken cancellationToken) =>
    {
        Result<BookingResponse> result = await sender.Send(new GetByIdBookingQuery(id), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
    });
}
