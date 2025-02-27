using Application.Specialists.Get;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Routing;
using Web.Api.Endpoints;

namespace Tests.Specialists;

public sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("specialists",
            async (GetSpecialistsQuery query, ISender sender, CancellationToken cancellationToken) =>
        {
            Result<List<SpecialistsResponse>> result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : MapErrorToResults.MapError(result.Error);
        });
}
