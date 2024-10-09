using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Groups;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Groups;

public sealed class AddMemberToGroupCommandHandler(IRepositoryBase<Group, Guid> groupRepository, IRepositoryBase<User, Guid> userRepository, IUnitOfWork unitOfWork) : ICommandHandler<Command.AddMemberToGroup>
{
    public async Task<Result> Handle(Command.AddMemberToGroup request, CancellationToken cancellationToken)
    {
        var U = await userRepository.FindByIdAsync(request.MemberId);
        if (U == null)
            return Result.Failure(new Error("404", "User Not Found"));
        var G = await groupRepository.FindSingleAsync(x => x.Id == request.GroupId, cancellationToken);
        if (G == null)
            return Result.Failure(new Error("404", "Group Not Found"));
        if (G.Members!.Any(x => x.StudentId == U.Id))
            return Result.Failure(new Error("422", "Member already joined group"));
        G.Members!.Add(new Group_Student_Mapping { StudentId = U.Id, GroupId = G.Id });
        groupRepository.Update(G);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
