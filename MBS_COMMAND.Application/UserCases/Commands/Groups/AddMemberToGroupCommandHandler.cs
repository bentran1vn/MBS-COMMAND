using MBS_COMMAND.Application.Abstractions;
using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Groups;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace MBS_COMMAND.Application.UserCases.Commands.Groups;

public sealed class AddMemberToGroupCommandHandler(
    IRepositoryBase<Group, Guid> groupRepository,
    IRepositoryBase<User, Guid> userRepository,
    IConfiguration configuration,
    IMailService mailService) : ICommandHandler<Command.AddMemberToGroup>
{
    public async Task<Result> Handle(Command.AddMemberToGroup request, CancellationToken cancellationToken)
    {
        var u = await userRepository.FindByIdAsync(request.MemberId, cancellationToken);
        if (u == null)
            return Result.Failure(new Error("404", "User Not Found"));
        var g = await groupRepository.FindSingleAsync(x => x.Id == request.GroupId, cancellationToken);
        if (g == null)
            return Result.Failure(new Error("404", "Group Not Found"));
        if (g.Members!.Any(x => x.StudentId == u.Id))
            return Result.Failure(new Error("422", "Member already joined group"));
        //g.Members!.Add(new Group_Student_Mapping { StudentId = u.Id, GroupId = g.Id });
        var domain = configuration["Domain"];
        await mailService.SendMail(new MailContent
        {
            To = u.Email,
            Subject = $"Invitation to group {g.Name}",
            Body = $@"
        <p>You have been invited to group {g.Name}</p>
        <a href='{domain}/api/v1/groups/accept-invitation/{g.Id}/{u.Id}' 
           style='display:inline-block; padding:10px 20px; background-color:#4CAF50; color:white; text-decoration:none; border-radius:5px;'>
           Accept Invitation
        </a>
    "
        });


        //send mail to user

        return Result.Success();
    }
}