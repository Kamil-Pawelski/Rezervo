using Application.Schedules;
using Application.Schedules.Get;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Schedule;

public sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) => 
    app.MapGet("schedules/",  async ([FromBody] GetScheduleQuery query, ISender sender, CancellationToken cancellationToken) =>
    {
        Result<List<ScheduleResponse>> result = await sender.Send(query, cancellationToken);
        return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
    });
}
