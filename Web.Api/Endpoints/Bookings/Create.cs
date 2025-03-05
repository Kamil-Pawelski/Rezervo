using Application.Bookings.Create;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Bookings;

public sealed class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("bookings/",
            async ([FromBody] CreateBookingCommand createBookingCommand, ISender sender,
                CancellationToken cancellationToken) =>
            {
                Result<string> result = await sender.Send(createBookingCommand, cancellationToken);

                return result.IsSuccess
                    ? Results.Created($"bookings/{result.Value}", result.Value)
                    : result.Error.MapError();
            });
}
