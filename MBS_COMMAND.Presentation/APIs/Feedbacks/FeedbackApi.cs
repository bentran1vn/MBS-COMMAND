using MBS_COMMAND.Contract.Services.Feedbacks;
using MBS_COMMAND.Presentation.Abstractions;

namespace MBS_COMMAND.Presentation.APIs.Feedbacks;

public class FeedbackApi : ApiEndpoint,ICarterModule
{
    private const string BaseUrl = "api/v{version:apiVersion}/feedbacks";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var gr1 = app.NewVersionedApi("Feedbacks").MapGroup(BaseUrl).HasApiVersion(1);
        gr1.MapPost(string.Empty, CreateFeedback).RequireAuthorization();
        
    }
    
    private static async Task<IResult> CreateFeedback(ISender sender, [FromBody] Command.CreateFeedback request)
    {
        var result = await sender.Send(request);
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }
}