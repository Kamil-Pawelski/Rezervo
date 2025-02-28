using Application.Specialists;
using Application.Specialists.GetById;
using Domain.Common;
using MediatR;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Specialists;

public class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
    app.MapGet("specialists/{id}", async (Guid id, ISender isender, CancellationToken cancellationToken) =>
    {
        var query = new GetByIdSpecialistQuery(id);

        Result<SpecialistsResponse> result = await isender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
    });
}
