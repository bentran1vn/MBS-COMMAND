using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Groups;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Groups;

public class ChangeLeaderCommandHandler(IRepositoryBase<User, Guid> userRepository, IRepositoryBase<Group, Guid> groupRepository, ICurrentUserService currentUserService, IUnitOfWork unitOfWork) : ICommandHandler<Command.ChangeLeader>
{
    public async Task<Result> Handle(Command.ChangeLeader request, CancellationToken cancellationToken)
    {
        var group = await groupRepository.FindByIdAsync(request.GroupId);
        if (group == null)
        {
            return Result.Failure(new Error("404", "User Not Found"));
        }
        if (currentUserService.UserId != group.LeaderId.ToString())
        {
            return Result.Failure(new Error("403", "You are not allowed to change the leader"));
        }
        var newLeader = await userRepository.FindByIdAsync(request.NewLeaderId);
        if (newLeader == null)
        {
            return Result.Failure(new Error("404", "User Not Found"));
        }
        group.LeaderId = newLeader.Id;
        groupRepository.Update(group);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
