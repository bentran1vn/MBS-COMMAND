using MBS_COMMAND.Contract.Services.Configs;
using MBS_COMMAND.Presentation.Abstractions;

namespace MBS_COMMAND.Presentation.APIs.Configs;
public class ConfigApi : ApiEndpoint, ICarterModule
{
    private const string _baseUrl = "/api/v{version:apiVersion}/configs";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var gr1 = app.NewVersionedApi("Configs")
            .MapGroup(_baseUrl).HasApiVersion(1);
        gr1.MapPost("generate-point", GeneratePoint);
        gr1.MapPost("generate-point-for-group", GeneratePointForGroup);
    }

    private static async Task<IResult> GeneratePoint(ISender sender)
    {
        var result = await sender.Send(new Command.GeneratePoints());

        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GeneratePointForGroup(ISender sender)
    {
        var result = await sender.Send(new Command.GeneratePointsForAllGroup());

        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }
}