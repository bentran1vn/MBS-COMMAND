using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Abstractions.Shared;
using MBS_COMMAND.Contract.Services.Groups;
using MBS_COMMAND.Domain.Abstractions.Repositories;
using MBS_COMMAND.Domain.Entities;

namespace MBS_COMMAND.Application.UserCases.Commands.Groups;

public sealed class CreateGroupCommandHandler : ICommandHandler<Command.CreateGroupCommand>
{
    private readonly IRepositoryBase<Group, Guid> _repositoryBase;

    public CreateGroupCommandHandler(IRepositoryBase<Group, Guid> repositoryBase)
    {
        _repositoryBase = repositoryBase;
    }

    public async Task<Result> Handle(Command.CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var G = Group.Create(request.Name, request.Stacks, request.MentorId);
        _repositoryBase.Add(G);
        return Result.Success();
    }
}
