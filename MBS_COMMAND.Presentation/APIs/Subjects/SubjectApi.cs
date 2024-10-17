using MBS_COMMAND.Contract.Services.Subjects;
using MBS_COMMAND.Presentation.Abstractions;

namespace MBS_COMMAND.Presentation.APIs.Subjects;

public class SubjectApi : ApiEndpoint,ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/subjects";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var gr1 = app.NewVersionedApi("Subjects").MapGroup(BaseUrl).HasApiVersion(1);
        gr1.MapPost(string.Empty, CreateSubject);
    }
    
    private static async Task<IResult> CreateSubject(ISender sender,[FromBody] Command.AddSubject command)
    {
        var result = await sender.Send(command);
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }
}