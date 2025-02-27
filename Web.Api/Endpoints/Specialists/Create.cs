using MediatR;

namespace Web.Api.Endpoints.Specialists;

public class Create : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app) => 
    app.MapPost("specialists", (CreateSpecialistCommand createSpecialistCommand, ISender iSender, CancellationToken cancellationToken) =>
        {

        }
    );
}

public sealed record CreateSpecialistCommand
{
}
