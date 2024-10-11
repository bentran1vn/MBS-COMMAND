using MBS_COMMAND.Contract.Services.Slots;
using MBS_COMMAND.Presentation.Abstractions;
namespace MBS_COMMAND.Presentation.APIs.Slots;
public class SlotApi : ApiEndpoint,ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/slots";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var gr1 = app.NewVersionedApi("Slots").MapGroup(BaseUrl).HasApiVersion(1);
        gr1.MapPost(string.Empty, CreateSlot);
    }
    public static async Task<IResult> CreateSlot(ISender sender, Command.CreateSlot createSlot)
    {
        var result = await sender.Send(createSlot);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}
