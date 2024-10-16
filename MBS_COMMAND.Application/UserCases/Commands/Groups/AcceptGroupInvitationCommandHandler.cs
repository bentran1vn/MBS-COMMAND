using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;
using MBS_CONTRACT.SHARE.Services.Groups;
using Command = MBS_COMMAND.Contract.Services.Groups.Command;

namespace MBS_COMMAND.Application.UserCases.Commands.Groups;

public class AcceptGroupInvitationCommandHandler(
    IRepositoryBase<User, Guid> userRepository,
    IRepositoryBase<Group, Guid> groupRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<Command.AcceptGroupInvitation>
{
    public async Task<Result> Handle(Command.AcceptGroupInvitation request, CancellationToken cancellationToken)
    {
        var u = await userRepository.FindByIdAsync(request.MemberId, cancellationToken);
        if (u == null)
            return Result.Failure(new Error("404", "User Not Found"));
        if (u.Status == 2)
        {
            return Result.Failure(new Error("403", "User is blocked"));
        }

        var g = await groupRepository.FindSingleAsync(x => x.Id == request.GroupId, cancellationToken);
        if (g == null)
            return Result.Failure(new Error("404", "Group Not Found"));
        if (g.Members!.Any(x => x.StudentId == u.Id))
            return Result.Failure(new Error("422", "Member already joined group"));
        g.Members!.Add(new Group_Student_Mapping { StudentId = u.Id, GroupId = g.Id });
        unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("Member added to group");
    }
}