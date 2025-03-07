using Application.Schedules;
using Application.Schedules.GetById;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Schedule;

public class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) => 
        app.MapGet("schedules/{id}", async ([FromRoute] Guid id, ISender sender, CancellationToken cancellationToken) =>
        {
            Result<List<SlotResponse>> result = await sender.Send(new GetByIdScheduleSlotsQuery(id) , cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
        });
}
