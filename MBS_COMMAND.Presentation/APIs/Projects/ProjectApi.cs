using MBS_COMMAND.Contract.Services.Projects;
using MBS_COMMAND.Presentation.Abstractions;

namespace MBS_COMMAND.Presentation.APIs.Projects;

public class ProjectApi : ApiEndpoint,ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/projects";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var gr1= app.NewVersionedApi("Projects").MapGroup(BaseUrl).HasApiVersion(1);
        gr1.MapPost(string.Empty, AddProject).RequireAuthorization();
    }
    private static async Task<IResult> AddProject(ISender sender, [FromBody] Command.AddProject request)
    {
        var result = await sender.Send(request);
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }
}