using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Groups;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Groups;

public class AddMentorToGroupCommandHandler(
    IRepositoryBase<User, Guid> userRepository,
    IRepositoryBase<Group, Guid> groupRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<Command.AddMentorToGroup>
{
    public async Task<Result> Handle(Command.AddMentorToGroup request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindByIdAsync(request.MentorId, cancellationToken);
        if (user == null)
        {
            return Result.Failure(new Error("404", "User not found"));
        }
        if(user.Role!=2)
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
        group.MentorId = request.MentorId;
        groupRepository.Update(group);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}