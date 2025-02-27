using Application.Specialists.Create;
using Domain.Common;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Web.Api.Endpoints.Specialists;

public class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) =>
    app.MapPost("specialists", 
        [Authorize(Roles = RolesNames.Specialist)]
        async (CreateSpecialistCommand createSpecialistCommand, ISender iSender, CancellationToken cancellationToken) =>
        {
            Result result = await iSender.Send(createSpecialistCommand, cancellationToken);

            return result.IsSuccess ? Results.Ok(new { Message = "The specialist account has been created." }) : Results.BadRequest();
        }
    );
}
