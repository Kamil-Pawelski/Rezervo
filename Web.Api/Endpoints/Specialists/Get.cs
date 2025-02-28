using Application.Specialists;
using Application.Specialists.Get;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Mapper;

namespace Web.Api.Endpoints.Specialists;

public sealed class Get : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("specialists",
            async (ISender sender, CancellationToken cancellationToken) =>
            {
                GetSpecialistsQuery query = new();
                Result<List<SpecialistsResponse>> result = await sender.Send(query, cancellationToken);

                return result.IsSuccess ? Results.Ok(result.Value) : result.Error.MapError();
            });
}

