using Application.Specialists;
using Application.Specialists.GetById;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Specialists;

public class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
    app.MapGet("specialists/{id}", async ([FromRoute]Guid id, ISender isender, CancellationToken cancellationToken) =>
    {
        var query = new GetByIdSpecialistQuery(id);

        Result<SpecialistsResponse> result = await isender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
    });
}
