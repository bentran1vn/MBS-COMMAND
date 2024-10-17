using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Groups;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Groups;

public sealed class CreateGroupCommandHandler(
    IRepositoryBase<Group, Guid> repositoryBase,
    IRepositoryBase<User, Guid> userRepository,
    ICurrentUserService currentUserService) : ICommandHandler<Command.CreateGroupCommand>
{
    public async Task<Result> Handle(Command.CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var T = Guid.TryParse(currentUserService.UserId, out var L);
        if (!T)
            return Result.Failure(new Error("404", "User Not Authorized"));
        var G = new Group
        {
            Name = request.Name,
            MentorId = request.MentorId,
            Stack = request.Stacks,
            // LeaderId = L
        };
        G.Members!.Add(new Group_Student_Mapping { StudentId = L, GroupId = G.Id });
        repositoryBase.Add(G);

        return Result.Success();
    }
}