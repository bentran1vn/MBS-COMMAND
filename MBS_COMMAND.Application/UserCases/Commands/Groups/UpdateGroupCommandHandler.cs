using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Groups;
using MBS_COMMAND.Domain.Abstractions;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Groups;

public class UpdateGroupCommandHandler(IRepositoryBase<Group, Guid> groupRepository, IUnitOfWork unitOfWork) : ICommandHandler<Command.UpdateGroup>
{
    public async Task<Result> Handle(Command.UpdateGroup request, CancellationToken cancellationToken)
    {
        var G = await groupRepository.FindByIdAsync(request.GroupId);
        if (G == null)
            return Result.Failure(new Error("404", "Group Not Found"));
        G.Name = request.Name;
        G.Stack = request.Stacks;
        groupRepository.Update(G);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
