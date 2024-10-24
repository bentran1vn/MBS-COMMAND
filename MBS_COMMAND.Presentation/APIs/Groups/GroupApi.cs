using MBS_COMMAND.Contract.Services.Groups;
using MBS_COMMAND.Presentation.Abstractions;

namespace MBS_COMMAND.Presentation.APIs.Groups;

public class GroupApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/groups";
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var gr1 = app.NewVersionedApi("Groups").MapGroup(BaseUrl).HasApiVersion(1);
        gr1.MapPost(string.Empty, CreateGroup).RequireAuthorization();
        gr1.MapPost("member", AddMemberToGroup);
        gr1.MapDelete("member", RemoveMemberFromGroup);
        gr1.MapPut("member", ChangeLeader).WithSummary("must login in order to use this api");
        gr1.MapPut(string.Empty, UpdateGroup);
        gr1.MapGet("accept-invitation/{groupId}/{memberId}", AcceptGroupInvitation);
        gr1.MapPost("mentor", AddMentorToGroup);
    }
    private static async Task<IResult> AddMentorToGroup(ISender sender, [FromBody] Command.AddMentorToGroup request)
    {
        var result = await sender.Send(request);
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }
    private static async Task<IResult> AcceptGroupInvitation(ISender sender, string groupId,string memberId)
    {
        var result = await sender.Send(new Command.AcceptGroupInvitation(Guid.Parse(groupId), Guid.Parse(memberId)));
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }
    private static async Task<IResult> UpdateGroup(ISender sender, [FromBody] Command.UpdateGroup updateGroup)
    {
        var result = await sender.Send(updateGroup);
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> ChangeLeader(ISender sender, [FromBody] Command.ChangeLeader request)
    {
        var result = await sender.Send(request);
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> CreateGroup(ISender sender, [FromBody] Command.CreateGroupCommand request)
    {
        var result = await sender.Send(request);
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> AddMemberToGroup(ISender sender, [FromBody] Command.AddMemberToGroup request)
    {
        var result = await sender.Send(request);
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> RemoveMemberFromGroup(ISender sender, [FromBody] Command.RemoveMemberFromGroup request)
    {
        var result = await sender.Send(request);
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }

}
