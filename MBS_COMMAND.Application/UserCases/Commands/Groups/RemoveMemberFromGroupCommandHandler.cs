using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Groups;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Groups;

public sealed class RemoveMemberFromGroupCommandHandler(IRepositoryBase<Group, Guid> groupRepository, IRepositoryBase<User, Guid> userRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : ICommandHandler<Command.RemoveMemberFromGroup>
{
    public async Task<Result> Handle(Command.RemoveMemberFromGroup request, CancellationToken cancellationToken)
    {
        var currentUserId = currentUserService.UserId;

        var U = await userRepository.FindByIdAsync(request.MemberId, cancellationToken);
        if (U == null)
            return Result.Failure(new Error("404", "User Not Found"));
        var G = await groupRepository.FindSingleAsync(x => x.Id == request.GroupId, cancellationToken);
        if (G == null)
            return Result.Failure(new Error("404", "Group Not Found"));
        if (currentUserId != null && G.LeaderId.ToString() == currentUserId)
            return Result.Failure(new Error("403", "Leader cannot be removed from group"));
        var member = G.Members!.FirstOrDefault(x => x.StudentId == U.Id);
        if (member == null)
            return Result.Failure(new Error("422", "Member are not found in group"));
        G.Members!.Remove(member);
        groupRepository.Update(G);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
