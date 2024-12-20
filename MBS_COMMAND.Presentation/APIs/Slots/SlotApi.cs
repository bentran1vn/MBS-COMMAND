﻿using MBS_COMMAND.Contract.Services.Slots;
using MBS_COMMAND.Presentation.Abstractions;

namespace MBS_COMMAND.Presentation.APIs.Slots;
public class SlotApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/slots";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var gr1 = app.NewVersionedApi("Slots").MapGroup(BaseUrl).HasApiVersion(1);
        gr1.MapPost(string.Empty, CreateSlot).WithSummary("mm/dd/yyyy").RequireAuthorization();
        gr1.MapPost("generate", GenerateSlotForSemester);
        gr1.MapPut(string.Empty, UpdateSlot).RequireAuthorization();
        gr1.MapDelete("{id}", DeleteSlot).RequireAuthorization();
    }

    private static async Task<IResult> DeleteSlot(ISender sender, Guid id)
    {
        var result = await sender.Send(new Command.DeleteSlot(id));
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }
    private static async Task<IResult> CreateSlot(ISender sender, [FromBody] Command.CreateSlot command)
    {
        var result = await sender.Send(command);
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateSlot(ISender sender, [FromBody] Command.UpdateSlot command)
    {
        var result = await sender.Send(command);
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }


    private static async Task<IResult> GenerateSlotForSemester(ISender sender)
    {
        var result = await sender.Send(new Command.GenerateSlotForSemester());
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }
}