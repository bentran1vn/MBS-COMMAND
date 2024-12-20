﻿using Carter;
using MBS_COMMAND.Contract.Services.Mentors;
using MBS_COMMAND.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace MBS_COMMAND.Presentation.APIs.User;

public class UserApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/user";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var gr1 = app.NewVersionedApi("User")
            .MapGroup(BaseUrl).HasApiVersion(1);

        gr1.MapPost("create-mentor", CreateUser);
        gr1.MapGet("mentor-accept-or-decline-from-group/groupId/mentorId/isAccepted", MentorAcceptOrDeclineFromGroup);

    }

    private static async Task<IResult> CreateUser(ISender sender, [FromBody] Command.CreateMentorCommand command)
    {
        var result = await sender.Send(command);

        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }
    private static async Task<IResult> MentorAcceptOrDeclineFromGroup(ISender sender,Guid mentorId,Guid groupId,bool isAccepted)
    {
        var result = await sender.Send(new Command.MentorAcceptOrDeclineFromGroupCommand(mentorId, isAccepted, groupId));
        return result.IsFailure ? HandlerFailure(result) : Results.Ok(result);
    }
   
}
