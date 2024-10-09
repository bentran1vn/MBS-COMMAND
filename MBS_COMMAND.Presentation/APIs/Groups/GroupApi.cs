using Carter;
using MBS_COMMAND.Contract.Services.Groups;
using MBS_COMMAND.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MBS_COMMAND.Presentation.APIs.Groups;

public class GroupApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/groups";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var gr1 = app.NewVersionedApi("Groups")
             .MapGroup(BaseUrl).HasApiVersion(1);
        gr1.MapPost(string.Empty, CreateGroup);
        gr1.MapPost("add-list-member", AddListMemberToGroup).WithSummary("add many at the time");
        gr1.MapDelete("remove-list-member", RemoveListMemberFromGroup).WithSummary("remove many at the time");
        gr1.MapPost("add-member", AddMemberToGroup);
        gr1.MapDelete("remove-member", RemoveMemberFromGroup);
    }


    public static async Task<IResult> CreateGroup(ISender sender, [FromBody] Command.CreateGroupCommand request)
    {
        var result = await sender.Send(request);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
    public static async Task<IResult> AddListMemberToGroup(ISender sender, [FromBody] Command.AddListMemberToGroup request)
    {
        var result = await sender.Send(request);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
    public static async Task<IResult> RemoveListMemberFromGroup(ISender sender, [FromBody] Command.RemoveListMemberFromGroup request)
    {
        var result = await sender.Send(request);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
    public static async Task<IResult> AddMemberToGroup(ISender sender, [FromBody] Command.AddMemberToGroup request)
    {
        var result = await sender.Send(request);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
    public static async Task<IResult> RemoveMemberFromGroup(ISender sender, [FromBody] Command.RemoveMemberFromGroup request)
    {
        var result = await sender.Send(request);
        if (result.IsFailure)
            return HandlerFailure(result);

        return Results.Ok(result);
    }
}
