using MBS_COMMAND.Application.Abstractions;
using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Groups;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;
using MBS_COMMAND.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MBS_COMMAND.Application.UserCases.Commands.Groups;
public class AddMentorToGroupCommandHandler(
    IRepositoryBase<User, Guid> userRepository,
    IRepositoryBase<Group, Guid> groupRepository,
    IUnitOfWork unitOfWork,
    ApplicationDbContext context,
    IConfiguration configuration,
    IMailService mailService)
    : ICommandHandler<Command.AddMentorToGroup>
{
    public async Task<Result> Handle(Command.AddMentorToGroup request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.MentorId, cancellationToken);
        if (user == null)
        {
            return Result.Failure(new Error("404", "User not found"));
        }

        if (user.Role != 1)
        {
            return Result.Failure(new Error("403", "User is not a mentor"));
        }

        var group = await groupRepository.FindByIdAsync(request.GroupId, cancellationToken);
        if (group == null)
        {
            return Result.Failure(new Error("404", "Group not found"));
        }

        if (group.MentorId is not null)
        {
            return Result.Failure(new Error("400", "Group already has a mentor"));
        }

        /*group.MentorId = request.MentorId;
        groupRepository.Update(group);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        await mailService.SendMail(new MailContent
        {
            To = user.Email,
            Subject = $"You have been added to group {group.Name} as a mentor",
        });*/
        var isInvited = await context.Configs.FirstOrDefaultAsync(x => x.Key.Equals($"{group.Name}MentorInvite"),
            cancellationToken);
        if (isInvited != null)
        {
            return Result.Failure(new Error("400", "Mentor already invited"));
        }

        var domain = configuration["Domain"];

        await mailService.SendMail(new MailContent
        {
            To = user.Email,
            Subject = $"You have been invited to group {group.Name} as a mentor",
            Body = $@"
        <p>Dear {user.FullName},</p>
        <p>You have been invited to join the group <strong>{group.Name}</strong> as a mentor.</p>
        <p>Please choose an option below:</p>

<a href='{{domain}}/api/v1/user/mentor-accept-or-decline-from-group/{{Uri.EscapeDataString(group.Id.ToString())}}/{{Uri.EscapeDataString(user.Id.ToString())}}/isAccepted?isAccepted=true'
   style='padding:10px 20px; color:#fff; background-color:green; text-decoration:none; border-radius:5px;'>
   Accept
</a>

<a href='{{domain}}/api/v1/user/mentor-accept-or-decline-from-group/{{Uri.EscapeDataString(group.Id.ToString())}}/{{Uri.EscapeDataString(user.Id.ToString())}}/isAccepted?isAccepted=false'
   style='padding:10px 20px; color:#fff; background-color:red; text-decoration:none; border-radius:5px; margin-left:10px;'>
   Decline
</a>

<p>Thank you!</p>

    "
        });

        var config = new Config
        {
            Key = $"{group.Name}MentorInvite",
            Value = "Pending"
        };
        context.Configs.Add(config);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}